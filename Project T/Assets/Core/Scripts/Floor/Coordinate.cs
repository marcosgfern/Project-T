using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floors {
/* Class Coordinate is used as the key in the Dictionary<Coordinate, Room> used by Floor and FloorGenerator classes.
 * Contains some helpful functions related to coordinates.
 */
    public class Coordinate {
        public int x, y;

        public Coordinate(int x, int y) {
            this.x = x;
            this.y = y;
        }

        /* Returns adjacent coordinates of this coordinate. */
        public Coordinate[] GetNeighbours() {
            Coordinate[] neighbours = new Coordinate[4];

            neighbours[0] = new Coordinate(this.x, this.y + 1);
            neighbours[1] = new Coordinate(this.x + 1, this.y);
            neighbours[2] = new Coordinate(this.x, this.y - 1);
            neighbours[3] = new Coordinate(this.x - 1, this.y);

            return neighbours;
        }

        /* Returns adjacent neighbour in @direction */
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

        /* Returns distance to coordinate [0, 0].
         * Distance is defined as the minimum quantity of steps 
         * necessary to get from one coordinate to another, 
         * moving only to an adjacent room in every step.
         */
        public int GetDistanceToOrigin() {
            return Mathf.Abs(this.x) + Mathf.Abs(this.y);
        }

        public bool IsOrigin() {
            return this.x == 0 
                && this.y == 0;
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
        public static Direction? GetOpposite(this Direction? direction) {
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
 