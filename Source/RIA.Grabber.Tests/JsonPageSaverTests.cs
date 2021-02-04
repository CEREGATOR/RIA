namespace RIA.Grabber.Tests
{
    using NUnit.Framework;
    using RIA.Grabber.Model;
    using RIA.Grabber.Services;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    [TestFixture]
    public class JsonPageSaverTests
    {
        private string _html;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _html = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "1.html"));
        }

        [Test]
        public void ParsePage_ReturnsExpectedModel()
        {
            //// Этап инициализации и подготовки ожидаемых данных. 
            //// В данном случае, Moq тебе не нужен, потому что у тестируемого класса нет зависимостей, от которых сдедует абстрагироваться.
            var parser = new HtmlParser();
            var expectedTitle = "Суд заменил Навальному условный срок на реальный по делу \"Ив Роше\"";
            var expectedText = "МОСКВА, 2 фев — РИА Новости. Симоновский суд Москвы отменил условный срок Алексея Навального по делу \"Ив Роше\" и заменил его тремя с половиной годами колонии, сообщил корреспондент РИА Новости.\r\nВыездное заседание проходило в зале Мосгорсуда.\r\nСудья Наталья Репникова объявила, что \"находит представление ФСИН подлежащим удовлетворению\".\r\nРепникова подчеркнула, что 12 месяцев, проведенные подсудимым под домашним арестом, зачтут в наказание по формуле \"день в день\". Таким образом, если защите не удастся оспорить решение, Навальный проведет в заключении меньше 3,5 года.\r\nПриговор в силу пока не вступил — у сторон есть десять дней на обжалование. Как сообщил РИА Новости адвокат Вадим Кобзев, защита воспользуется этим правом.\r\nТакже суд вынес частное постановление в адрес инспектора ФСИН, следившего за соблюдением Навальным правил условного осуждения. По мнению прокуратуры, он плохо справился со своей работой. Судья призвала \"обратить внимание начальника филиала на допущенные нарушения\".\r\nРанее Навальному удалось трижды избежать замены условного срока на реальный. Отказные решения выносил Люблинский суд Москвы в 2015 и 2016 годах, а Симоновский суд Москвы в 2017-м также лишь продлил ему испытательный срок, не ставя вопрос об отправке в колонию.\r\nНавального задержали 17 января в Шереметьево по возвращении из Германии, где он проходил лечение. \r\nОснованием для ареста стало нарушение испытательного срока по делу \"Ив Роше\". Как заявили во ФСИН, Навального задержали за нарушения испытательного срока как условно осужденного — 29 декабря прошлого года его объявили в розыск. \r\nУ Навального есть две условные судимости: по делу \"Кировлеса\" — она связана с растратой более 16 миллионов рублей и по делу \"Ив Роше\" — с хищением более 30 миллионов рублей. \r\nНавальный обращался в Европейский суд по правам человека по обоим делам, ЕСПЧ не нашел доказательств, что они были политически мотивированными, но постановил, что дела рассматривались с нарушениями. Присужденные ЕСПЧ суммы компенсаций были полностью выплачены.";
            var expectedPublicationDate = new DateTime(2021, 02, 02, 20, 22, 00);
            var expectedLastChangeDate = new DateTime(2021, 02, 02, 21, 13, 00);
            var expLinks = new List<LinkWithDescription>
            {
                new LinkWithDescription { Url = "http://ria.ru/person_Aleksejj_Navalnyjj/", Description = "Алексея Навального" },
                new LinkWithDescription { Url = "http://ria.ru/organization_Federalnaja_sluzhba_ispolnenija_nakazanijj_RF/", Description = "ФСИН" },
                new LinkWithDescription { Url = "http://ria.ru/location_Moskva/", Description = "Москвы" },
                new LinkWithDescription { Url = "http://ria.ru/organization_SHeremetevo/", Description = "Шереметьево" },
                new LinkWithDescription { Url = "http://ria.ru/location_Germany/", Description = "Германии" },
                new LinkWithDescription { Url = "http://ria.ru/organization_ARRAY_0x69adbc0/", Description = "Ив Роше"},
                new LinkWithDescription { Url = "http://ria.ru/organization_Kirovles/", Description = "Кировлеса"},
                new LinkWithDescription { Url = "http://ria.ru/organization_ARRAY_0x8271018/", Description = "Европейский суд"},
            };

            //// Нужно подобрать другой пример, в котором действительно будут картинки.
            var expImgLinks = new List<string>();

            //// Этап вызова.
            var actual = parser.ParsePage(_html);

            //// Этап проверки результатов.
            Assert.NotNull(actual);
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
        }

    }
}
