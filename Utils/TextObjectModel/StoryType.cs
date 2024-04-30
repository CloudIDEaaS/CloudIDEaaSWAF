using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.TextObjectModel
{
    public enum StoryType
    {
        UnknownStory = 0,
        MainTextStory = 1,
        FootnotesStory = 2,
        EndnotesStory = 3,
        CommentsStory = 4,
        TextFrameStory = 5,
        EvenPagesHeaderStory = 6,
        PrimaryHeaderStory = 7,
        EvenPagesFooterStory = 8,
        PrimaryFooterStory = 9,
        FirstPageHeaderStory = 10,
        FirstPageFooterStory = 11,
        CustomStory = 12
    }
}
