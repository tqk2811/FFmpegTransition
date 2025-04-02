using FFmpegArgs.Cores.Maps;
using System;

namespace FFmpegTransition.Interfaces
{
    public interface ITransition
    {
        bool IsConcat { get; }
        ImageMap MakeTransition(ImageMap first_imageMap, ImageMap second_imageMap, TimeSpan totalDuration, double fps);
    }
}
