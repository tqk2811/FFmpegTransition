﻿using FFmpegArgs.Cores.Enums;
using FFmpegArgs.Cores.Maps;
using FFmpegArgs.Filters.VideoFilters;
using FFmpegTransition.Interfaces;
using System;

namespace FFmpegTransition.Transitions
{
    public class CrossFadeTransition : ITransition
    {
        public bool IsConcat { get; } = false;

        public ImageMap MakeTransition(ImageMap first_imageMap, ImageMap second_imageMap, TimeSpan totalDuration, double fps)
        {
            var fade_out = first_imageMap
                .FormatFilter(PixFmt.argb)
                    .MapOut
                .FadeFilter()
                    .Type(FadeType.Out)
                    .StartFrame(0)
                    .Alpha(true)
                    .NbFrames((int)(totalDuration.TotalSeconds * fps))
                    .MapOut;
            var fade_in = second_imageMap
                .FormatFilter(PixFmt.argb)
                    .MapOut
                .FadeFilter()
                    .Type(FadeType.In)
                    .StartFrame(0)
                    .Alpha(true)
                    .NbFrames((int)(totalDuration.TotalSeconds * fps))
                    .MapOut;
            return fade_out.OverlayFilterOn(fade_in).Format(OverlayPixFmt.yuv420).MapOut;
        }
    }
}
