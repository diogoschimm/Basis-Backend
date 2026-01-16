using CadastroLivros.Api.Controllers.Bases;
using CadastroLivros.Infra.DbContexts;
using FastReport;
using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Composition;
using System.Data;

namespace CadastroLivros.Api.Controllers;

/// <summary>
/// Controller para geração de relatórios
/// </summary>
public class ReportsController(IConfiguration configuration) : ApiControllerBase
{
    /// <summary>
    /// Exporta o relatório de Autores e Livros em PDF
    /// </summary>
    /// <returns>Arquivo PDF do relatório</returns>
    [HttpGet("autores-livros/pdf")]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ExportarRelatorioAutoresLivrosPdf()
    {
        try
        {
            RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));
            var reportPath = Path.Combine(AppContext.BaseDirectory, "reports", "RelatorioAutoresLivros.frx");

            if (!System.IO.File.Exists(reportPath))
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Relatorio.NaoEncontrado",
                    detail: $"Arquivo de relatório não encontrado: {reportPath}"
                );
            }

            var report = new Report();
            report.Load(reportPath);

            var connection = report.Dictionary.Connections
                  .OfType<MsSqlDataConnection>()
                  .FirstOrDefault();

            if (connection == null)
                throw new Exception("Conexão SQL não encontrada no relatório.");

            connection.ConnectionString = configuration.GetConnectionString("DefaultConnection");
            connection.Enabled = true;
            report.Dictionary.ReRegisterData();
            report.Prepare();

            // Exportar para PDF
            var pdfExport = new PDFSimpleExport();
            using var stream = new MemoryStream();
            report.Export(pdfExport, stream);
            stream.Position = 0;

            var fileName = $"RelatorioAutoresLivros_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            return File(stream.ToArray(), "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Relatorio.Erro",
                detail: $"Erro ao gerar relatório: {ex.Message}"
            );
        }
    }
}

