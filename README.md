# wordpress-to-hugo-and-staticman-converter
Convert a [Wordpress][1] [xml export][2] to [Hugo][3] posts and [Staticman][4] comment-files.
[![Build status](https://ci.appveyor.com/api/projects/status/pyrjhl3ltq4e4owd?svg=true)](https://ci.appveyor.com/project/xs4free/wordpress-to-hugo-and-staticman-converter)

# Credits
Based my code on [wordpress-to-hugo-exporter][5] from [Cyrill Schumacher][6] which is a Wordpress plugin (php) to export posts to Hugo files.
Added staticman comments files based on [Hugo + Staticman: Nested Replies and E-mail Notifications][7] by [Dan C Williams][8]. 

# TODO
- [x] Add AppVeyor build tag
- [x] Write comments to own file
- [x] Convert post and comment HTML to Markdown
- [x] Add banner-property to Hugo-markdown based on Wordpress postmeta-key "_thumbnail_id"
- [x] Save blogs in [Page Resources/Bundles][9] instead of all .md files in one directory
- [ ] Add option to generate images using [Hugo custom shortcode (for example 'imgproc')][12] instead of regular markdown for an image
- [ ] Build using [Microsoft Azure DevOps][10] instead of AppVeyor
- [ ] Parse caption-tag in post-content; convert to figcaption
- [ ] Parse gallery-tag in post-content into page-resources (specified in the header) 
- [ ] Improve WordpressWXR12 library with enums for field values
- [ ] Extract WordpressWXR12 library into it's own library and NuGet-package
- [ ] Validate images referenced in posts are presents in static content folder
- [ ] Validate links in posts referencing images in site are present in static content folder (example 'DSC01373.jpg' in '2010/09/27/terugvlucht-naar-nederland')
- [ ] Remove context dependency from AutoMapper Profile class (ConverterLibraryAutoMapperProfile) using [DI example by Jan de Vries][11]

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