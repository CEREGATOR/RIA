namespace RIA.Grabber.Tests
{
    using NUnit.Framework;
    using RIA.Grabber.Model;
    using RIA.Grabber.Services;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    [TestFixture]
    public class JsonPageSaverTests
    {
        private string _html;
        private string _dirPath;
        private string _filePath;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _html = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "2.html"));
            _dirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        [Test]
        public void ParsePage_ReturnsExpectedModel()
        {
            var parser = new HtmlParser();
            var json = new JsonPageSaver();
            var expectedNameFile = "Цены на нефть в 2020 году упали на 20% из-за пандемии";
            var expectedTitle = "Цены на нефть в 2020 году упали на 20% из-за пандемии";
            var expectedText = "МОСКВА, 31 дек - РИА Новости. Мировые цены на нефть опускаются в четверг днем, при этом с начала года котировки упали на 20% на фоне пандемии коронавируса, свидетельствуют данные торгов.\r\nПо состоянию на 12.39 мск цена мартовских фьючерсов на североморскую нефтяную смесь марки Brent снижалась на 0,35% - до 51,45 доллара за баррель, февральских фьючерсов на WTI - на 0,33%, до 48,24 доллара за баррель. С начала года котировки опустились в среднем на 21-22%, в начале года нефть стоила больше 60 долларов за баррель.\r\nВ текущем году пандемия коронавируса стала главной темой на мировых рынках. Всемирная организация здравоохранения (ВОЗ) 11 марта объявила вспышку коронавируса пандемией. Ограничительные меры во всем мире привели к падению мировой экономики, в результате из-за ожиданий по спросу нефтяные цены упали.\r\nСамое сильное за год месячное падение котировок нефти было зарегистрировано в марте. Страны ОПЕК+ 6 марта не смогли договориться ни об изменении параметров соглашения о сокращении добычи нефти, ни о его продлении. В результате ограничения на добычу в странах-участницах прежнего альянса с 1 апреля были сняты. Позднее странам альянса удалось заключить новое соглашение, которое учитывает влияние второй волны пандемии.\r\nНефтяные цены 6 марта упали на 10%, а 9 марта - на 25%, после объявления пандемии снижение котировок продолжилось. Суммарно за март нефть марок WTI и Brent подешевела почти вдвое - примерно с 44 и 49 долларов до 21 и 23 долларов соответственно. 20 апреля нефть марки WTI с поставкой в мае впервые в истории упала до отрицательных значений, котировки на Нью-Йоркской фьючерсной товарной бирже (NYMEX) достигали минус 40,32 доллара за баррель.\r\nВесной и летом цены на WTI и Brent частично восстановились, в сентябре и октябре котировки сдержала вторая волна пандемии, а в ноябре цены выросли на 27% из-за новостей о вакцине. Теперь рынки ожидают заседания ОПЕК+, которые запланированы на ближайшие дни. Заседание ОПЕК+ по условиям сделки альянса с февраля состоится 4 января, а накануне пройдет встреча министерского мониторингового комитета, который вынесет рекомендации по будущим параметрам соглашения.";
            DateTime? expectedPublicationDate = new DateTime(2020, 12, 31, 13, 02, 00);
            DateTime? expectedLastChangeDate = null;
            var expLinks = new List<LinkWithDescription>
            {
                new LinkWithDescription { Url = "http://ria.ru/product_Brent_Crude/", Description = "Brent" },
                new LinkWithDescription { Url = "http://ria.ru/organization_Vsemirnaja_organizacija_zdravookhranenija/", Description = "ВОЗ" },
                new LinkWithDescription { Url = "http://ria.ru/organization_Organizacija_stran_proizvoditelejj_i_ehksporterov_nefti/", Description = "ОПЕК+" }
            };

            var expImgLinks = new List<string>()
            {
                "https://cdn25.img.ria.ru/images/07e4/07/1c/1575062950_0:183:2815:1766_1920x0_80_0_0_dfd7a0d1fdebf8e965e5724d8ce41a1c.jpg.webp"
            };

            var expImagesInBase64 = new List<string>();

            var pageModel = parser.ParsePage(_html);
            json.SavePageModel(pageModel, _dirPath);
            _filePath = json.filePath;
            var jsonString = File.ReadAllText(_filePath);
            var actual =  JsonSerializer.Deserialize<PageModel>(jsonString);

            Assert.NotNull(actual);
            Assert.AreEqual(expectedNameFile, Path.GetFileNameWithoutExtension(_filePath));
            Assert.AreEqual(expectedTitle, actual.Title);

            Assert.AreEqual(expectedText, actual.Text);
            Assert.AreEqual(expectedPublicationDate, actual.PublicationDate);
            Assert.AreEqual(expectedLastChangeDate, actual.LastChangeDate);

            Assert.AreEqual(expLinks.Count, actual.LinksInText.Count);
            for (var i = 0; i < expLinks.Count; i++)
            {
                Assert.AreEqual(expLinks[i].Url, actual.LinksInText[i].Url);
                Assert.AreEqual(expLinks[i].Description, actual.LinksInText[i].Description);
            }

            Assert.AreEqual(expImgLinks.Count, actual.ImageLinks.Count);
            for (var i = 0; i < actual.ImageLinks.Count; i++)
            {
                Assert.AreEqual(expImgLinks[i], actual.ImageLinks[i]);
            }

            Assert.AreEqual(expImagesInBase64.Count, actual.ImagesInBase64.Count);
            for (var i = 0; i < actual.ImagesInBase64.Count; i++)
            {
                Assert.AreEqual(expImagesInBase64[i], actual.ImagesInBase64[i]);
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            File.Delete(_filePath);
        }
    }
}
