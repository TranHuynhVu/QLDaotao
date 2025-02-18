using DinkToPdf;
using DinkToPdf.Contracts;

namespace QLDaoTao.Areas.Admin.Services
{
    public class ItemPDFService : IPDF
    {
        private readonly IConverter _converter;

        public ItemPDFService(IConverter converter)
        {
            _converter = converter;
        }

        public async Task<byte[]> GeneratePdfFromHtml(string htmlContent)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Landscape
                
            },
                Objects = {
                new ObjectSettings {
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" }
                }
            }
            };

            return _converter.Convert(doc);
        }
    }
}
