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
- [ ] Parse caption-tag in post-content; convert to figcaption
- [ ] Parse gallery-tag in post-content
- [ ] Improve WordpressWXR12 library with enums for field values
- [ ] Extract WordpressWXR12 library into it's own library and NuGet-package
- [ ] Validate images referenced in posts are presents in static content folder

[1]: https://wordpress.com/
[2]: https://en.support.wordpress.com/export/
[3]: https://gohugo.io/
[4]: https://staticman.net
[5]: https://github.com/SchumacherFM/wordpress-to-hugo-exporter
[6]: https://twitter.com/SchumacherFM
[7]: https://networkhobo.com/2017/12/30/hugo---staticman-nested-replies-and-e-mail-notifications/
[8]: http://twitter.com/dancwilliams