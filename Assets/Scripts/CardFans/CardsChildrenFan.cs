using UnityEngine;


namespace CardFans
{
    public class CardsChildrenFan : MonoBehaviour
    {
        [SerializeField]
        private Vector3 positionMergin;

        [SerializeField]
        private Vector3 rotationMergin;

        [SerializeField]
        private float zAngle;

        [SerializeField]
        private float alignSpeed;

        [SerializeField]
        private float rotateAlignSpeed;
        
        public bool IsLayoutUpdating { get; private set; }


        private void Update()
        {
            if (IsLayoutUpdating)
                CalculateLayout();
        }

        private void CalculateLayout()
        {
            bool isDone = true;

            if (this.transform.childCount > 0)
            {
                int childCount = this.transform.childCount;
                int childIndex = 0;

                float angleStep = zAngle / childCount;

                Vector3 nextCardPosition = new Vector3(- positionMergin.x * childCount / 2, positionMergin.y, positionMergin.z);
                Vector3 nextCardRotation = new Vector3(0, -zAngle / 2, 0) + rotationMergin;

                foreach (Transform child in this.transform)
                {
                    if (Quaternion.Angle(child.transform.localRotation, Quaternion.Euler(new Vector3(0, nextCardRotation.y, child.localEulerAngles.z))) > 2)
                    {
                        isDone = false;
                        child.localRotation = Quaternion.Slerp(Quaternion.Euler(child.localEulerAngles), Quaternion.Euler(new Vector3(0, nextCardRotation.y, child.localEulerAngles.z)), Time.deltaTime * rotateAlignSpeed);
                    }

                    nextCardRotation.y -= angleStep;

                    float zPos = 0;

                    if (childIndex < childCount)
                        zPos = angleStep * 0.01f * (float)childIndex;
                    else
                        zPos = angleStep * 0.01f * (float)(childIndex - childCount);

                    nextCardPosition.z += zPos;

                    if (child.localPosition != nextCardPosition)
                    {
                        isDone = false;
                        child.localPosition = Vector3.MoveTowards(child.localPosition, nextCardPosition, alignSpeed * Time.deltaTime);
                    }

                    nextCardPosition += positionMergin;

                    childIndex++;
                }

                IsLayoutUpdating = !isDone;
            }
        }

        public void Add(GameObject card)
        {
            card.transform.SetParent(this.transform);
        }

        public void Remove(GameObject card)
        {
            if (card.transform.IsChildOf(this.transform))
                card.transform.SetParent(null);
        }

        public void UpdateLayout()
        {
            IsLayoutUpdating = true;
        }
    }
}
