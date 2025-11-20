using FFmpegArgs.Cores.Maps;
using FFmpegArgs.Filters.Multimedia;
using FFmpegArgs.Filters.VideoFilters;
using System;
using System.Linq;
using TqkLibrary.FFmpegTransition.Interfaces;

namespace TqkLibrary.FFmpegTransition.Transitions
{
    public class FadeInTwoTransition : ITransition
    {
        public ImageMap MakeTransition(ImageMap first_imageMap, ImageMap second_imageMap, TimeSpan totalDuration, double fps)
        {
            TimeSpan stepDuration = TimeSpan.FromSeconds(totalDuration.TotalSeconds / 2);
            var fade_out = first_imageMap.FadeFilter().Type(FadeType.Out).StartFrame(0).NbFrames((int)(stepDuration.TotalSeconds * fps)).MapOut;
            var fade_in = second_imageMap.FadeFilter().Type(FadeType.In).StartFrame(0).NbFrames((int)(stepDuration.TotalSeconds * fps)).MapOut;
            ConcatFilter concatFilter = new(new ConcatGroup(fade_out), new ConcatGroup(fade_in));
            return concatFilter.ImageMapsOut.First();//.TrimFilter().Duration(totalDuration).MapOut;
        }
    }
}
