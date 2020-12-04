using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using Colors_reduction.OctreeTree;
using System.Drawing;

namespace UnitTest
{
    [TestClass]
    public class OctreeTest
    {
        [TestMethod]
        public void BlackAndWhiteReductionToOneColor()
        {
            Octree octree = new Octree();
            octree.AddColor(Color.FromArgb(0, 0, 0));
            octree.AddColor(Color.FromArgb(255, 255, 255));
            octree.ReduceColorsCount(1);
            Color reductedColor = octree.GetColor(Color.FromArgb(0, 0, 0));
            Assert.AreEqual(reductedColor, Color.FromArgb(127, 127, 127));
        }

        [TestMethod]
        public void BlackAndTwoWhiteReductionToOneColor()
        {
            Octree octree = new Octree();
            octree.AddColor(Color.FromArgb(0, 0, 0));
            octree.AddColor(Color.FromArgb(255, 255, 255));
            octree.AddColor(Color.FromArgb(255, 255, 255));
            octree.ReduceColorsCount(1);
            Color reductedColor = octree.GetColor(Color.FromArgb(0, 0, 0));
            Assert.AreEqual(reductedColor, Color.FromArgb(170, 170, 170));
        }

        [TestMethod]
        public void AddNewAndGetNewColor()
        {
            Octree octree = new Octree();
            octree.AddColor(Color.FromArgb(0, 0, 0));
            Assert.AreEqual(octree.GetColor(Color.FromArgb(0, 0, 0)), Color.FromArgb(0, 0, 0));
        }


        [TestMethod]
        public void GetColorCounts()
        {
            Octree octree = new Octree();
            octree.AddColor(Color.FromArgb(0, 0, 0));
            octree.AddColor(Color.FromArgb(0, 0, 0));
            octree.AddColor(Color.FromArgb(0, 0, 1));
            octree.AddColor(Color.FromArgb(0, 1, 0));
            octree.AddColor(Color.FromArgb(1, 0, 0));
            octree.AddColor(Color.FromArgb(0, 0, 0));
            octree.AddColor(Color.FromArgb(0, 0, 2));
            octree.AddColor(Color.FromArgb(255, 255, 255));
            octree.AddColor(Color.FromArgb(255, 255, 255));
            octree.AddColor(Color.FromArgb(54, 23, 54));
            octree.AddColor(Color.FromArgb(21, 213, 144));
            octree.AddColor(Color.FromArgb(124, 43, 255));
            Assert.AreEqual(octree.ColorsCount, 9);
        }
    }
}
