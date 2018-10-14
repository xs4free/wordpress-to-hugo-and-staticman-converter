# wordpress-to-hugo-and-staticman-converter
Convert a [WordPress][1] [xml export][2] to [Hugo][3] posts and [Staticman][4] comment-files.
[![Build status](https://ci.appveyor.com/api/projects/status/pyrjhl3ltq4e4owd?svg=true)](https://ci.appveyor.com/project/xs4free/wordpress-to-hugo-and-staticman-converter)

# How to run
## Commandline options

Short | Long | Description
------|------|------------
-i | --input             |Required. Location of XML export from WordPress to convert.
-o | --output            |Required. Location where hugo-files will be written.
-c | --content           |Location(s) of wp-contents folder(s) where images from WordPress can be found.
-p | --pageresources     |Use page resources as location to store images (instead of 'uploads' folder).
-s | --shortcodeimage    |Hugo Shortcode used for rendering images in front-matter. If not specified a Markdown image tag will be used.
- | --help                |Display help screen.
- |--version              |Display version information.

## Example
`dotnet wordpress-to-hugo-and-staticman-converter.dll -i "..\blogname.wordpress.2018-08-01.xml" -o "..\Output" -c "..\wp-content","..\wp-content\weblog" -pageresources -s "img \"{0}\" \"{1}\"  \"{2}\""`

# Credits
Based my code on [wordpress-to-hugo-exporter][5] from [Cyrill Schumacher][6] which is a WordPress plugin (php) to export posts to Hugo files.
Added staticman comments files based on [Hugo + Staticman: Nested Replies and E-mail Notifications][7] by [Dan C Williams][8]. 

# TODO
- [ ] Build using [Microsoft Azure DevOps][10] instead of AppVeyor
- [ ] Parse caption-tag in post-content; convert to figcaption
- [ ] Replace '<span style="font-size: 13.3333px;">e</span>' in text with Markdown unrelated to font-size
- [ ] Rename WordpressWXR12 library to WordPressWXR12 
- [ ] Improve WordpressWXR12 library with enums for field values
- [ ] Extract WordpressWXR12 library into it's own library and NuGet-package
- [ ] Validate images referenced in posts are presents in static content folder
- [ ] Validate links in posts referencing images in site are present in static content folder (example 'DSC01373.jpg' in '2010/09/27/terugvlucht-naar-nederland')
- [ ] Remove context dependency from AutoMapper Profile class (ConverterLibraryAutoMapperProfile) using [DI example by Jan de Vries][11]
- [x] ~~Add AppVeyor build tag~~
- [x] ~~Write comments to own file~~
- [x] ~~Convert post and comment HTML to Markdown~~
- [x] ~~Add banner-property to Hugo-markdown based on WordPress postmeta-key "_thumbnail_id"~~
- [x] ~~Save blogs in [Page Resources/Bundles][9] instead of all .md files in one directory~~
- [x] ~~Parse gallery-tag in post-content into page-resources (specified in the header)~~ 
- [x] ~~Add option to generate images using [Hugo custom shortcode (for example 'imgproc')][12] instead of regular markdown for an image~~
- [x] ~~Fix spaces in image names bacause Hugo crashes when these are used in Page Resources~~
- [x] ~~Check location of images when not using shortcode~~
- [x] ~~Check location of images when not using Page Resources~~

[1]: https://wordpress.com/
[2]: https://en.support.wordpress.com/export/
[3]: https://gohugo.io/
[4]: https://staticman.net
[5]: https://github.com/SchumacherFM/wordpress-to-hugo-exporter
[6]: https://twitter.com/SchumacherFM
[7]: https://networkhobo.com/2017/12/30/hugo---staticman-nested-replies-and-e-mail-notifications/
[8]: http://twitter.com/dancwilliams
[9]: https://regisphilibert.com/blog/2018/01/hugo-page-resources-and-how-to-use-them/
[10]: https://azure.microsoft.com/services/devops/
[11]: https://jan-v.nl/post/using-dependency-injection-in-your-automapper-profile
[12]: https://gohugo.io/about/new-in-032/