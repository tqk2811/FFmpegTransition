using FFmpegArgs.Filters.VideoFilters;
using FFmpegTransition.Enums;
using FFmpegTransition.Interfaces;
using FFmpegTransition.Transitions;
using System.Collections.Generic;
using System.IO;

namespace FFmpegTransition.Test
{
    [TestClass]
    public class TransitionTest
    {
        readonly TimeSpan duration;
        readonly TimeSpan halfDuration;
        public TransitionTest()
        {
            duration = TimeSpan.FromSeconds(2);
            halfDuration = TimeSpan.FromSeconds(duration.TotalSeconds / 2);
        }

        void Render(ITransition translation, params string[] filenames)
        {
            var names = new List<string>()
            {
                translation.GetType().Name,
            };
            names.AddRange(filenames);

            TimeSpan itemDuration = translation.IsConcat ? halfDuration : duration;

            FFmpegArg ffmpegArg = new FFmpegArg().OverWriteOutput();

            ImageMap first_map = ffmpegArg
                .AddImagesInput(new ImageFileInput(".\\Resources\\Acro Trip ED.mp4").Duration(itemDuration))
                .First()
                .FpsFilter()
                    .Fps(24).MapOut;

            ImageMap second_map = ffmpegArg
                .AddImagesInput(new ImageFileInput(".\\Resources\\Mayonaka Punch OP.mp4").Duration(itemDuration))
                .First()
                .FpsFilter()
                    .Fps(24).MapOut;

            ImageMap imageMap = translation.MakeTransition(first_map, second_map, duration, 24);
            imageMap = imageMap.TrimFilter().Duration(duration).MapOut;

            ffmpegArg.AddOutput(new ImageFileOutput($"{string.Join(".", names)}.mp4", imageMap).AndSet(x => x.ImageOutputAVStream.Fps(24)));

            string workingDir = Directory.GetCurrentDirectory();
            ffmpegArg.Render(x => x.WithWorkingDirectory(workingDir)).Execute().EnsureSuccess();
        }

        [TestMethod]
        public void FadeInTwoTest()
        {
            Render(new FadeInTwoTransition());
        }

        [TestMethod]
        public void CrossFadeTest()
        {
            Render(new CrossFadeTransition());
        }

        [TestMethod]
        public void ExpandCircularTest()
        {
            Render(new ExpandTransition(CollapseExpandMode.Circular), nameof(CollapseExpandMode.Circular));
        }
        [TestMethod]
        public void ExpandVerticalTest()
        {
            Render(new ExpandTransition(CollapseExpandMode.Vertical), nameof(CollapseExpandMode.Vertical));
        }
        [TestMethod]
        public void ExpandHorizontalTest()
        {
            Render(new ExpandTransition(CollapseExpandMode.Horizontal), nameof(CollapseExpandMode.Horizontal));
        }
        [TestMethod]
        public void ExpandBothTest()
        {
            Render(new ExpandTransition(CollapseExpandMode.Both), nameof(CollapseExpandMode.Both));
        }

        [TestMethod]
        public void CollapseCircularTest()
        {
            Render(new CollapseTransition(CollapseExpandMode.Circular), nameof(CollapseExpandMode.Circular));
        }
        [TestMethod]
        public void CollapseVerticalTest()
        {
            Render(new CollapseTransition(CollapseExpandMode.Vertical), nameof(CollapseExpandMode.Vertical));
        }
        [TestMethod]
        public void CollapseHorizontalTest()
        {
            Render(new CollapseTransition(CollapseExpandMode.Horizontal), nameof(CollapseExpandMode.Horizontal));
        }
        [TestMethod]
        public void CollapseBothTest()
        {
            Render(new CollapseTransition(CollapseExpandMode.Both), nameof(CollapseExpandMode.Both));
        }

        [TestMethod]
        public void ClockTest()
        {
            Render(new ClockTransition());
        }
        [TestMethod]
        public void ClockContraryTest()
        {
            Render(new ClockTransition() { Contrary = true }, nameof(ClockTransition.Contrary));
        }
    }
}