﻿/*
 * The MIT License (MIT)
 * Copyright (c) StarX 2015 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CrazyStorm.Core
{
    public enum ParticleColor
    {
        None,
        Red,
        Purple,
        Blue,
        Cyan,
        Green,
        Yellow,
        Orange,
        Gray
    }
    public class ParticleType : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Private Members
        string name;
        Resource image;
        Vector2 startPoint;
        int width;
        int height;
        Vector2 centerPoint;
        int frames;
        int delay;
        int radius;
        ParticleColor color;
        #endregion

        #region Public Members
        public string Name
        {
            get { return name; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value)) name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }
        public Resource Image
        {
            get { return image; }
            set
            {
                image = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Image"));
            }
        }
        public Vector2 StartPoint
        {
            get { return startPoint; }
            set
            {
                startPoint = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("StartPoint"));
            }
        }
        public float StartPointX
        {
            get { return startPoint.x; }
            set
            {
                startPoint.x = value >= 0 ? value : 0;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("StartPointX"));
                    PropertyChanged(this, new PropertyChangedEventArgs("CircleBoxX"));
                }
            }
        }
        public float StartPointY
        {
            get { return startPoint.y; }
            set
            {
                startPoint.y = value >= 0 ? value : 0;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("StartPointY"));
                    PropertyChanged(this, new PropertyChangedEventArgs("CircleBoxY"));
                }
            }
        }
        public int Width
        {
            get { return width; }
            set
            {
                width = value >= 0 ? value : 0;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Width"));
            }
        }
        public int Height
        {
            get { return height; }
            set
            {
                height = value >= 0 ? value : 0;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Height"));
            }
        }
        public Vector2 CenterPoint
        {
            get { return centerPoint; }
            set
            {
                centerPoint = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CenterPoint"));
            }
        }
        public float CenterPointX
        {
            get { return centerPoint.x; }
            set
            {
                centerPoint.x = value >= 0 ? value : 0;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CenterPointX"));
                    PropertyChanged(this, new PropertyChangedEventArgs("CircleBoxX"));
                }
            }
        }
        public float CircleBoxX
        {
            get { return StartPointX + CenterPointX - Radius; }
        }
        public float CenterPointY
        {
            get { return centerPoint.y; }
            set
            {
                centerPoint.y = value >= 0 ? value : 0;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CenterPointY"));
                    PropertyChanged(this, new PropertyChangedEventArgs("CircleBoxY"));
                }
            }
        }
        public float CircleBoxY
        {
            get { return StartPointY + CenterPointY - Radius; }
        }
        public int Frames
        {
            get { return frames; }
            set
            {
                frames = value >= 1 ? value : 1;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Frames"));
            }
        }
        public int Delay
        {
            get { return delay; }
            set
            {
                delay = value >= 0 ? value : 0;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Delay"));
            }
        }
        public int Radius
        {
            get { return radius; }
            set
            {
                radius = value >= 0 ? value : 0;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Radius"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Diameter"));
                    PropertyChanged(this, new PropertyChangedEventArgs("CircleBoxX"));
                    PropertyChanged(this, new PropertyChangedEventArgs("CircleBoxY"));
                }
            }
        }
        public int Diameter
        { get { return radius * 2; }}
        public ParticleColor Color
        {
            get { return color; }
            set
            {
                color = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Color"));
            }
        }
        #endregion

        #region Constructor
        public ParticleType()
        {
            name = "Untitled";
            startPoint = Vector2.Zero;
            centerPoint = Vector2.Zero;
            frames = 1;
        }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}