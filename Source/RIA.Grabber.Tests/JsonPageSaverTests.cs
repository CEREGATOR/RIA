namespace RIA.Grabber.Tests
{
    using NUnit.Framework;
    using Model;
    using Services;
    using System;
    using System.Collections.Generic;

    using Moq;

    using JsonSerializer = System.Text.Json.JsonSerializer;

    [TestFixture]
    public class JsonPageSaverTests
    {
        [Test]
        public void SavePageModel_WritesCorrectSerializedModel()
        {
            var expectedModel = new PageModel
            {
                ImagesInBase64 = new List<string> { "1" },
                LastChangeDate = DateTime.Now,
                LinksInText = new List<LinkWithDescription>
                {
                    new LinkWithDescription { Description = "1", Url = "1" },
                    new LinkWithDescription { Description = "2", Url = "2" },
                },
                PublicationDate = DateTime.Now.AddHours(-1),
                Text = "Text",
                Title = "Title",
            };

            string fileContent = null;

            var jsonMock = new Mock<JsonPageSaver>();
            jsonMock.Setup(x => x.WriteFileContent(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((p, c) => fileContent = c);
            jsonMock.Setup(x => x.CreateDirIfNotExists(It.IsAny<string>()));

            var json = jsonMock.Object;

            var dirPath = "C:\\";
            var filePath = $"{dirPath}{expectedModel.Title}.json";
            json.SavePageModel(expectedModel, dirPath);

            jsonMock.Verify(saver => saver.CreateDirIfNotExists(dirPath), Times.Once);
            jsonMock.Verify(saver => saver.WriteFileContent(filePath, It.IsAny<string>()), Times.Once);

            Assert.NotNull(fileContent);
            Assert.AreEqual(JsonSerializer.Serialize(expectedModel), fileContent);
        }
    }
}
