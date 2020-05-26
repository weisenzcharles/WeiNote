using DotNetCodeFirst.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCodeFirst.Database
{
    public class MovieDbContextSeeder
    {
        public static void Seed(MovieContext context)
        {

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Movies.Add(new Movie()
            {
                Name = "唐人街探案",
                Description = "《唐人街探案》（英语：Detective Chinatown）是一部 2015 年的中国喜剧悬疑电影，由陈思诚执导兼编剧，王宝强、刘昊然主演，主要在泰国取景拍摄。该片讲述了两名华人在泰国陷入了刑案风暴，在短短三天内找到下落不明的黄金及查明杀人真凶，洗刷冤屈的故事。在 2016 年第 53 届金马奖获得共 5 项提名。",
                PreviewImage = "https://bkimg.cdn.bcebos.com/pic/cb8065380cd79123ea03474aaa345982b2b78029",
                ReleaseDate = DateTime.Parse("2015-12-31"),
                Maker = "万达影视传媒有限公司",
                Duration = "02:15:00",
                Director = "陈思诚",
                MovieCategories = new List<MovieCategory>
                {
                    new MovieCategory(){ Category = new Category() { Name = "悬疑", Description = "悬疑片是一种电影类型，其故事情节围绕着解决一个问题或侦破一项犯罪行为。其核心通常是侦探，包括私家侦探和业余侦探，借助于线索、调查和聪明的推理，来努力解决一个问题的神秘状况。" }},
                    new MovieCategory(){ Category = new Category() { Name = "喜剧", Description = "喜剧是戏剧的一种类型，大众一般解作笑剧或趣剧，以夸张的手法、巧妙的结构、诙谐的台词及对喜剧性格的刻画，从而引人对丑的、滑稽的予以嘲笑，对正常的人生和美好的理想予以肯定。" }},
                },
                Screenshots = new List<Screenshot>
                {
                    new Screenshot() { Url = "https://bkimg.cdn.bcebos.com/pic/adaf2edda3cc7cd98ed36fe43101213fb80e9114" },
                    new Screenshot() { Url = "https://bkimg.cdn.bcebos.com/pic/9d82d158ccbf6c81b31076d5b43eb13533fa40e3" },
                    new Screenshot() { Url = "https://bkimg.cdn.bcebos.com/pic/a1ec08fa513d269719d7d0c550fbb2fb4316d826" },
                    new Screenshot() { Url = "https://bkimg.cdn.bcebos.com/pic/902397dda144ad3422dc5f1fd5a20cf430ad85c3" },
                    new Screenshot() { Url = "https://bkimg.cdn.bcebos.com/pic/d0c8a786c9177f3eadc1249575cf3bc79e3d56b5" },
                },
                MovieActors = new List<MovieActor>()
                {
                    new MovieActor(){ Actor = new Actor() { Name = "王宝强", Description = "王宝强，1982 年 4 月 29 日出生于河北省邢台市，中国内地男演员、导演。王宝强 6 岁开始练习武术，8 岁在嵩山少林寺做俗家弟子。" }},
                    new MovieActor(){ Actor = new Actor() { Name = "刘昊然", Description = "刘昊然，1997 年 10 月 10 日出生于河南省平顶山市，中国内地男演员，就读于中央戏剧学院表演系本科。" }},
                    new MovieActor(){ Actor = new Actor() { Name = "佟丽娅", Description = "佟丽娅，1983 年 8 月 8 日出生于新疆伊犁察布查尔，中国内地影视女演员、舞者。2006 年，佟丽娅因出演情感剧《新不了情》而踏入演艺圈。" }},
                    new MovieActor(){ Actor = new Actor() { Name = "陈赫", Description = "陈赫，1985 年 11 月 9 日出生于福建省福州市长乐区，毕业于上海戏剧学院表演系，上海话剧艺术中心演员，中国内地男演员、歌手、主持人。" }},
                    new MovieActor(){ Actor = new Actor() { Name = "小沈阳", Description = "小沈阳，本名沈鹤，1981 年 5 月 7 日生于中国辽宁省铁岭市开原市，中国男演员、歌手、导演、主持人。" }},
                    new MovieActor(){ Actor = new Actor() { Name = "肖央", Description = "肖央，1980 年 4 月 7 日出生于河北省承德市，毕业于北京电影学院美术系，中国内地男导演、演员、歌手、编剧。" }},
                    new MovieActor(){ Actor = new Actor() { Name = "潘粤明", Description = "潘粤明，1974 年 5 月 9 日出生于北京市西城区，中国内地男演员，毕业于北京师范大学艺术系影视制作专业。1994 年参演个人首部电视剧《三国演义》饰演吴国第三任皇帝孙休，正式进入影视圈。" }},
                    new MovieActor(){ Actor = new Actor() { Name = "张子枫", Description = "张子枫，2001 年 8 月 27 日出生于河南省三门峡市，中国内地女演员。在 5 岁时，张子枫开始拍广告并因参演多部电视剧从而进入演艺圈。" }},
                }
            });

            context.Movies.Add(new Movie()
            {
                Name = "天下无贼",
                Description = "《天下无贼》是由冯小刚执导，刘德华、刘若英、葛优、王宝强等人主演的剧情片。该片于 2004 年 12 月 9 日在中国大陆上映。该片根据赵本夫的同名小说改编而成，讲述了一对扒窃搭档在一趟列车中为了实现一个名叫傻根的民工“天下无贼”的愿望，便与另一个扒窃团伙展开了一系列明争暗斗的故事。",
                PreviewImage = "https://bkimg.cdn.bcebos.com/pic/c83d70cf3bc79f3dfaa0607cb4a1cd11728b293c",
                ReleaseDate = DateTime.Parse("2004-12-09"),
                Maker = "华谊兄弟",
                Duration = "02:00:00",
                Director = "冯小刚",
                MovieCategories = new List<MovieCategory>
                {
                    new MovieCategory(){ Category = new Category() { Name = "犯罪", Description = "犯罪片是世界上最受欢迎的电影或电视剧类型之一，所有剧情影视当中，犯罪片必定有犯罪也有警探侦办；与黑帮电影不同的是：犯罪片侧重在警探捉拿罪犯，阻止犯罪及犯罪可恶不道德所有剧情过程；黑帮电影则侧重在集会结社之自由权利，及帮派徒众争权夺利合理化及英雄化义气化，警探侦防犯罪则是剧情支线，甚至没有警探；犯罪片及黑帮电影都属于动作片，皆为商业电影提供大众购买观赏及娱乐。" }},
                    new MovieCategory(){ Category = new Category() { Name = "喜剧", Description = "喜剧是戏剧的一种类型，大众一般解作笑剧或趣剧，以夸张的手法、巧妙的结构、诙谐的台词及对喜剧性格的刻画，从而引人对丑的、滑稽的予以嘲笑，对正常的人生和美好的理想予以肯定。" }},
                },
                Screenshots = new List<Screenshot>
                {
                    new Screenshot() { Url = "https://bkimg.cdn.bcebos.com/pic/adaf2edda3cc7cd98ed36fe43101213fb80e9114" },
                    new Screenshot() { Url = "https://bkimg.cdn.bcebos.com/pic/9d82d158ccbf6c81b31076d5b43eb13533fa40e3" },
                    new Screenshot() { Url = "https://bkimg.cdn.bcebos.com/pic/a1ec08fa513d269719d7d0c550fbb2fb4316d826" },
                    new Screenshot() { Url = "https://bkimg.cdn.bcebos.com/pic/902397dda144ad3422dc5f1fd5a20cf430ad85c3" },
                    new Screenshot() { Url = "https://bkimg.cdn.bcebos.com/pic/d0c8a786c9177f3eadc1249575cf3bc79e3d56b5" },
                },
                MovieActors = new List<MovieActor>()
                {
                    new MovieActor(){ Actor = new Actor() { Name = "刘德华", Description = "刘德华（Andy Lau），1961 年 9 月 27 日出生于中国香港，籍贯广东新会，中国香港男演员、歌手、作词人、制片人。" }},
                    new MovieActor(){ Actor = new Actor() { Name = "刘若英", Description = "刘若英（Rene Liu），1969 年 6 月 1 日出生于台湾省台北市， 中国台湾女歌手、演员、导演、词曲创作者，毕业于美国加州州立大学音乐系。" }},
                    new MovieActor(){ Actor = new Actor() { Name = "李冰冰", Description = "李冰冰（Li Bingbing），中国内地影视女演员。1973 年 2 月 27 日出生于黑龙江省哈尔滨市五常市，1997 年毕业于上海戏剧学院表演系本科班。" }},
                    new MovieActor(){ Actor = new Actor() { Name = "葛优", Description = "葛优，1957 年 4 月 19 日出生于北京，中国内地男演员，国家一级演员。1979 年考入中华全国总工会文工团。" }},
                    new MovieActor(){ Actor = new Actor() { Name = "王宝强", Description = "王宝强，1982 年 4 月 29 日出生于河北省邢台市，中国内地男演员、导演。王宝强 6 岁开始练习武术，8 岁在嵩山少林寺做俗家弟子。" }},
                }
            });

            context.SaveChanges();
        }
    }
}
