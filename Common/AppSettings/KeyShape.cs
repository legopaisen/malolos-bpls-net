using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Amellar.Common.AppSettings
{
    public class KeyShape
    {

        public GraphicsPath KeyShapeDesign()
        {
            GraphicsPath path = new GraphicsPath();
            Point[] pointKey = new Point[] 
                { 
                new Point(57, 13),
                new Point(312,13),
                new Point(313,14),
                new Point(314, 14),
                new Point(315,15),
                new Point(316,16),
                new Point(317, 17),
                new Point(318,18),
                new Point(319,19),
                new Point(319, 20),
                new Point(320,22),
                new Point(321, 25),
                new Point(321,49),
                new Point(344,65),
                new Point(473, 65),
                new Point(489,81),
                new Point(350, 81),
                new Point(350, 85),
                new Point(495, 85),
                new Point(495, 100),
                new Point(351, 100),
                new Point(351, 104),
                new Point(495, 104),
                new Point(494, 111),
                new Point(493, 116),
                new Point(470, 135),
                new Point(461, 129),
                new Point(346, 130),
                new Point(344, 134),
                new Point(342, 138),
                new Point(340, 142),
                new Point(339, 144),
                new Point(337, 149),
                new Point(337, 169),
                new Point(335, 172),
                new Point(322, 172),
                new Point(322, 189),
                new Point(320, 193),
                new Point(319, 195),
                new Point(318, 197),
                new Point(317, 199),
                new Point(315, 200),
                new Point(313, 201),
                new Point(311, 202),
                new Point(307, 203),
                new Point(307, 204),
                new Point(60, 204),
                new Point(57, 203),
                new Point(55, 202),
                new Point(53, 200),

                new Point(49, 196),
                new Point(49, 194),
                new Point(47, 191),
                new Point(47, 161),
                new Point(17, 142),
                new Point(17, 66),
                new Point(47, 49),
                new Point(47, 24),
                new Point(49, 21),
                new Point(50, 19),
                new Point(55, 14),
                new Point(57, 13)
                };

            Point[] pointKeyIn = new Point[] 
            {
                new Point(47, 74),
                new Point(42, 74),
                new Point(37, 75),
                new Point(32, 76),
                new Point(27, 77),

                new Point(27, 129),
                new Point(32, 131),
                new Point(37, 132),
                new Point(42, 134),
                new Point(47, 135),
                new Point(47, 74),
            
            };




            path.AddPolygon(pointKey);
            path.AddPolygon(pointKeyIn);

            return path;
        }

        //added by Joelar Larion
        public GraphicsPath KeyShapeDesignLogin()
        {
            GraphicsPath path = new GraphicsPath();
            Point[] pointKey = new Point[] 
                { 
                new Point(5, 115),
                new Point(6,114),
                new Point(6,105),
                new Point(11, 101),
                new Point(18,101),
                new Point(18,94),
                new Point(23,90),
                new Point(26,90),

              new Point(26,87),
              new Point(27,88),
              new Point(27,85),
              new Point(45,67),
              new Point(44,63),
              new Point(47,59),
              new Point(46,54),

                new Point(46,41),
                new Point(47,38),
                new Point(48,36),
                new Point(49,34),

                new Point(50,32),
                new Point(52,30),

                new Point(53,28),
                new Point(59,23),
                new Point(66,20),
                new Point(69,19),
                                    
                new Point(78,19),
                new Point(86,22),
                new Point(96,16),



                new Point(101,14),
                new Point(106,13),
                new Point(106,9),
                new Point(114,1),
                new Point(243,1),


                new Point(265,17),
                new Point(408,17),
                new Point(411,18),
                new Point(413,19),
                new Point(417,22),
                new Point(420,27),
                new Point(421,31),
                new Point(421,144),
                new Point(419,149),
                new Point(417,152),
                new Point(413,155),
                new Point(409,156),
                new Point(131,156),
                new Point(125,154),
                new Point(122,150),
                new Point(119,146),
                new Point(118,142),
                new Point(118,110),

                new Point(106,90),
                new Point(106,76),
                new Point(103,78),
                new Point(103,138),
                new Point(98,142),
                new Point(96,143),
                new Point(96,144),
                new Point(93,147),
                new Point(92,145),
                new Point(87,139),


                new Point(87,132),
                new Point(90,129),
                new Point(90,126),
                new Point(87,122),
                new Point(87,118),
                new Point(89,116),
                new Point(89,113),
                new Point(87,110),
                new Point(87,84),
                new Point(82,81),


                new Point(81,76),
                new Point(67,75),
                new Point(59,74),
                new Point(17,116),
                new Point(15,116),
                new Point(13,115),
                new Point(8,114),
                new Point(5,115)
                };




            Point[] pointKeyIn1 = new Point[] 
            {
                new Point(58, 33),
                new Point(64, 40),
                new Point(68, 36),
                new Point(76, 33),
                new Point(76, 27),
                new Point(66, 26),
                new Point(63, 28),
                new Point(58, 33),
            };

            Point[] pointKeyIn2 = new Point[] 
            {
                new Point(71, 46),
                new Point(76, 52),
                new Point(93, 52),
                new Point(93, 48),
                new Point(87, 46),
                new Point(83, 43),
                new Point(80, 39),
                new Point(80, 35),
                new Point(72, 42),
                new Point(71, 46)
            };

            Point[] pointKeyIn3 = new Point[] 
            {
                new Point(85, 35),
                new Point(84, 40),
                new Point(87, 43),
                new Point(94, 44),
                new Point(93, 40),
                new Point(90, 34),
                new Point(87, 32),
                new Point(85, 35),
            };

            Point[] pointKeyIn4 = new Point[] 
            {
                new Point(97, 32),
                new Point(99, 36),
                new Point(100, 41),
                new Point(105, 39),
                new Point(106, 36),
                new Point(103, 33),
                new Point(100, 31),
                new Point(96, 31),
                new Point(97, 32),
            };

            Point[] pointKeyIn5 = new Point[] 
            {
                new Point(101, 47),
                new Point(101, 52),
                new Point(106, 52),
                new Point(106, 45),
                new Point(101, 47)
            };

            Point[] pointKeyIn6 = new Point[] 
            {
                new Point(106, 25),
                new Point(106, 19),
                new Point(103, 19),
                new Point(98, 22),
                new Point(102, 24)
            };



            Point[] pointKeyIn7 = new Point[] 
            {
                new Point(112, 13),
                new Point(114, 9),
                new Point(117, 7),
                new Point(121, 7),
                new Point(125, 10),
                new Point(126, 14),
                new Point(125, 18),
                new Point(122, 21),

                new Point(119, 18),
                new Point(116, 16),
                new Point(112, 13)
            };








            path.AddPolygon(pointKey);

            path.AddPolygon(pointKeyIn1);
            path.AddPolygon(pointKeyIn2);
            path.AddPolygon(pointKeyIn3);
            path.AddPolygon(pointKeyIn4);
            path.AddPolygon(pointKeyIn5);
            path.AddPolygon(pointKeyIn6);
            path.AddPolygon(pointKeyIn7);


            return path;
        }

    }
}
