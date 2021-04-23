using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floors {
    public class Coordinate {
        public int x, y;

        public Coordinate(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public Coordinate[] GetNeighbours() {
            Coordinate[] neighbours = new Coordinate[4];

            neighbours[0] = new Coordinate(this.x, this.y + 1);
            neighbours[1] = new Coordinate(this.x + 1, this.y);
            neighbours[2] = new Coordinate(this.x, this.y - 1);
            neighbours[3] = new Coordinate(this.x - 1, this.y);

            return neighbours;
        }

        public Coordinate GetNeighbour(Direction direction) {
            switch (direction) {
                case Direction.Up:
                    return new Coordinate(this.x, this.y + 1);

                case Direction.Right:
                    return new Coordinate(this.x + 1, this.y);

                case Direction.Down:
                    return new Coordinate(this.x, this.y - 1);

                case Direction.Left:
                    return new Coordinate(this.x - 1, this.y);

                default:
                    return null;
            }
        }

        public int DistanceToOrigin() {
            return Mathf.Abs(this.x) + Mathf.Abs(this.y);
        }

        override public string ToString() {
            return "[" + this.x + "," + this.y + "]";
        }

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }

            if (!(obj is Coordinate)) {
                return false;
            }

            return this.x == ((Coordinate)obj).x
                && this.y == ((Coordinate)obj).y;
        }

        public override int GetHashCode() {
            return (this.x + "," + this.y).GetHashCode();
        }
    }

    public enum Direction : int {
        Up,
        Right,
        Down,
        Left
    }
    public static class DirectionExtensions {
        public static Direction? Opposite(this Direction? direction) {
            switch (direction) {
                case Direction.Up: 
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                default:
                    return null;
            }
        }
    }
}
 