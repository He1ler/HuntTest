using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        public float yPos = .5f; 
        float distanceTravelled;
        Vector3 curPos = Vector3.zero;
        Quaternion curRot = Quaternion.identity;
        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;
                curPos = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                curPos.y = yPos;
                curRot = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                curRot = SetZRotation(curRot, 0f);
                transform.SetPositionAndRotation(curPos, curRot);
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
        private Quaternion SetZRotation(Quaternion originalRotation, float newZRotation)
        {
            Vector3 eulerRotation = originalRotation.eulerAngles;
            eulerRotation.z = newZRotation;
            return Quaternion.Euler(eulerRotation);
        }
    }
}