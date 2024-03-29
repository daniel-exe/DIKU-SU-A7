﻿using DIKUArcade.Entities;

namespace DIKUArcade.Graphics {
    public interface IBaseImage {
        void Render(Shape shape);
        void Render(Shape shape, Camera camera);
    }
}