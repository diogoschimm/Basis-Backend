using CadastroLivros.Api.Controllers.Bases;
using CadastroLivros.Infra.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using FastReport.Export.PdfSimple;

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
            var reportPath = Path.Combine(AppContext.BaseDirectory, "reports", "RelatorioAutoresLivros.frx");

            if (!System.IO.File.Exists(reportPath))
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Relatorio.NaoEncontrado",
                    detail: $"Arquivo de relatório não encontrado: {reportPath}"
                );
            }

            var report = new FastReport.Report();
            report.Load(reportPath);

            // Obter dados da view usando DataTable
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var dataTable = new DataTable();

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var command = new SqlCommand("SELECT * FROM VW_RelatorioPorAutor", connection);
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(dataTable);

            // Registrar o DataTable no relatório 
            report.RegisterData(dataTable, "rpt_AutoresLivros");
            report.GetDataSource("rpt_AutoresLivros")!.Enabled = true;

            // Preparar o relatório
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

