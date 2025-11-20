using FFmpegArgs.Cores.Maps;
using System;

namespace FFmpegTransition.Interfaces
{
    public interface ITransition
    {
        ImageMap MakeTransition(ImageMap first_imageMap, ImageMap second_imageMap, TimeSpan totalDuration, double fps);
    }
}
