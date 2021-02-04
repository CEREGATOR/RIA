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
    public class HtmlParserTests
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
            var parser = new HtmlParser();
            var expectedTitle = "��� ������� ���������� �������� ���� �� �������� �� ���� \"�� ����\"";
            var expectedText = "������, 2 ��� � ��� �������. ����������� ��� ������ ������� �������� ���� ������� ���������� �� ���� \"�� ����\" � ������� ��� ����� � ��������� ������ �������, ������� ������������� ��� �������.\r\n�������� ��������� ��������� � ���� ����������.\r\n����� ������� ��������� ��������, ��� \"������� ������������� ���� ���������� ��������������\".\r\n��������� �����������, ��� 12 �������, ����������� ���������� ��� �������� �������, ������ � ��������� �� ������� \"���� � ����\". ����� �������, ���� ������ �� ������� �������� �������, ��������� �������� � ���������� ������ 3,5 ����.\r\n�������� � ���� ���� �� ������� � � ������ ���� ������ ���� �� �����������. ��� ������� ��� ������� ������� ����� ������, ������ ������������� ���� ������.\r\n����� ��� ����� ������� ������������� � ����� ���������� ����, ���������� �� ����������� ��������� ������ ��������� ���������. �� ������ �����������, �� ����� ��������� �� ����� �������. ����� �������� \"�������� �������� ���������� ������� �� ���������� ���������\".\r\n����� ���������� ������� ������ �������� ������ ��������� ����� �� ��������. �������� ������� ������� ���������� ��� ������ � 2015 � 2016 �����, � ����������� ��� ������ � 2017-� ����� ���� ������� ��� ������������� ����, �� ����� ������ �� �������� � �������.\r\n���������� ��������� 17 ������ � ����������� �� ����������� �� ��������, ��� �� �������� �������. \r\n���������� ��� ������ ����� ��������� �������������� ����� �� ���� \"�� ����\". ��� ������� �� ����, ���������� ��������� �� ��������� �������������� ����� ��� ������� ����������� � 29 ������� �������� ���� ��� �������� � ������. \r\n� ���������� ���� ��� �������� ���������: �� ���� \"���������\" � ��� ������� � ��������� ����� 16 ��������� ������ � �� ���� \"�� ����\" � � �������� ����� 30 ��������� ������. \r\n��������� ��������� � ����������� ��� �� ������ �������� �� ����� �����, ���� �� ����� �������������, ��� ��� ���� ����������� ���������������, �� ����������, ��� ���� ��������������� � �����������. ������������ ���� ����� ����������� ���� ��������� ���������.";
            var expectedPublicationDate = new DateTime(2021, 02, 02, 20, 22, 00);
            var expectedLastChangeDate = new DateTime(2021, 02, 02, 21, 13, 00);
            var expLinks = new List<LinkWithDescription>
            {
                new LinkWithDescription { Url = "http://ria.ru/person_Aleksejj_Navalnyjj/", Description = "������� ����������" },
                new LinkWithDescription { Url = "http://ria.ru/organization_Federalnaja_sluzhba_ispolnenija_nakazanijj_RF/", Description = "����" },
                new LinkWithDescription { Url = "http://ria.ru/location_Moskva/", Description = "������" },
                new LinkWithDescription { Url = "http://ria.ru/organization_SHeremetevo/", Description = "�����������" },
                new LinkWithDescription { Url = "http://ria.ru/location_Germany/", Description = "��������" },
                new LinkWithDescription { Url = "http://ria.ru/organization_ARRAY_0x69adbc0/", Description = "�� ����"},
                new LinkWithDescription { Url = "http://ria.ru/organization_Kirovles/", Description = "���������"},
                new LinkWithDescription { Url = "http://ria.ru/organization_ARRAY_0x8271018/", Description = "����������� ���"},
            };

            var expImgLinks = new List<string>();

            var actual = parser.ParsePage(_html);

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