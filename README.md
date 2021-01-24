# RIA

c # program (.net462 or higher / .netcore3.1), which accepts as input a link to the news on the https://ria.ru/ resource and the path to the directory where the output file should be saved.

The result of the program: File (xml or json, as you prefer), created in the specified directory.
The file should describe the news and contain:
- News headline;
- News text, stripped of all links, pictures, etc.;
- Date of publication of the news;
- The image located at the beginning of the news, if any. The image must be Base64 encoded. If you do not know what Base64 is, then a file with a picture should be created next to the news file, which has the same name as the news file.
- Links contained in the text of the news. Each link must contain a text title and the link itself.

The file name is the title of the news. For example: "A rally in support of the law on state language.xml has begun in Kiev." If the title is longer than 70 characters, then use only the first 70 characters.
