﻿// <copyright file="LinearLineSegment.cs" company="Scott Williams">
// Copyright (c) Scott Williams and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Shaper2D
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Numerics;

    /// <summary>
    /// Represents a series of control points that will be joined by straight lines
    /// </summary>
    /// <seealso cref="ILineSegment" />
    public class LinearLineSegment : ILineSegment
    {
        /// <summary>
        /// The collection of points.
        /// </summary>
        private readonly ImmutableArray<Point> points;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearLineSegment"/> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        public LinearLineSegment(Point start, Point end)
            : this(new[] { start, end })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearLineSegment" /> class.
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <param name="additionalPoints">Additional points</param>
        public LinearLineSegment(Point point1, Point point2, params Point[] additionalPoints)
            : this(new[] { point1, point2 }.Merge(additionalPoints))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearLineSegment"/> class.
        /// </summary>
        /// <param name="points">The points.</param>
        public LinearLineSegment(Point[] points)
        {
            Guard.NotNull(points, nameof(points));
            Guard.MustBeGreaterThanOrEqualTo(points.Count(), 2, nameof(points));

            this.points = ImmutableArray.Create(points);

            this.EndPoint = points[points.Length - 1];
        }

        /// <summary>
        /// Gets the end point.
        /// </summary>
        /// <value>
        /// The end point.
        /// </value>
        public Point EndPoint { get; private set; }

        /// <summary>
        /// Converts the <see cref="ILineSegment" /> into a simple linear path..
        /// </summary>
        /// <returns>
        /// Returns the current <see cref="ILineSegment" /> as simple linear path.
        /// </returns>
        public ImmutableArray<Point> Flatten()
        {
            return this.points;
        }

        /// <summary>
        /// Transforms the current LineSegment using specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>
        /// A line segment with the matrix applied to it.
        /// </returns>
        public ILineSegment Transform(Matrix3x2 matrix)
        {
            if (matrix.IsIdentity)
            {
                // no transform to apply skip it
                return this;
            }

            var points = new Point[this.points.Length];
            var i = 0;
            foreach (var p in this.points)
            {
                points[i++] = p.Transform(matrix);
            }

            return new LinearLineSegment(points);
        }
    }
}