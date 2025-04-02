﻿using FFmpegArgs.Cores.Maps;
using FFmpegArgs.Filters;
using FFmpegArgs.Filters.VideoFilters;
using FFmpegTransition.Enums;
using FFmpegTransition.Interfaces;
using System;

namespace FFmpegTransition.Transitions
{
    public class CollapseTransition : ITransition
    {
        public bool IsConcat { get; } = false;
        readonly CollapseExpandMode collapseExpandMode;
        public CollapseTransition(CollapseExpandMode collapseExpandMode)
        {
            this.collapseExpandMode = collapseExpandMode;
        }


        public ImageMap MakeTransition(ImageMap first_imageMap, ImageMap second_imageMap, TimeSpan totalDuration, double fps)
        {
            double TRANSITION_DURATION = totalDuration.TotalSeconds;
            ImageMap imageMap = null;
            switch (this.collapseExpandMode)
            {
                case CollapseExpandMode.Circular:
                    imageMap = second_imageMap
                        .GeqFilter()
                            .Lum("p(X,Y)")
                            .A($"if(lte(pow(sqrt(pow(W/2,2)+pow(H/2,2))-sqrt(pow(T/{TRANSITION_DURATION}*W/2,2)+pow(T/{TRANSITION_DURATION}*H/2,2)),2),pow(X-(W/2),2)+pow(Y-(H/2),2)),255,0)").MapOut;
                    imageMap = imageMap.OverlayFilterOn(first_imageMap).MapOut;
                    break;

                case CollapseExpandMode.Horizontal:
                case CollapseExpandMode.Vertical:
                case CollapseExpandMode.Both:
                    {
                        string expr = this.collapseExpandMode switch
                        {
                            CollapseExpandMode.Vertical => $"if(gte(Y,(H/2)*T/{TRANSITION_DURATION})*lte(Y,H-(H/2)*T/{TRANSITION_DURATION}),B,A)",
                            CollapseExpandMode.Horizontal => $"if(gte(X,(W/2)*T/{TRANSITION_DURATION})*lte(X,W-(W/2)*T/{TRANSITION_DURATION}),B,A)",
                            CollapseExpandMode.Both => $"if((gte(X,(W/2)*T/{TRANSITION_DURATION})*gte(Y,(H/2)*T/{TRANSITION_DURATION}))*(lte(X,W-(W/2)*T/{TRANSITION_DURATION})*lte(Y,H-(H/2)*T/{TRANSITION_DURATION})),B,A)",
                            _ => throw new NotFiniteNumberException(this.collapseExpandMode.ToString()),
                        };

                        imageMap = second_imageMap
                            .BlendFilterOn(first_imageMap)
                                .Shortest(true)
                                .All_Expr(expr)
                                .MapOut;
                    }
                    break;

                default:
                    throw new NotFiniteNumberException(this.collapseExpandMode.ToString());
            }

            return imageMap;
        }
    }
}
