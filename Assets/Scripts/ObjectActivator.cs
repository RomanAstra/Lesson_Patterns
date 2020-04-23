using UnityEngine;


namespace DefaultNamespace
{
    public sealed class ObjectActivator : MonoBehaviour
    {
        [SerializeField] private TagType _tagType;
        [SerializeField] private bool _deactivateOnExit;
        [SerializeField] private GameObject[] _objects;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(TagManager.GetTag(_tagType)))
            {
                Activation(true);
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_deactivateOnExit && collision.CompareTag(TagManager.GetTag(_tagType)))
            {
                Activation(false);
            }
        }

        private void Activation(bool ActiveObj)
        {
            for (var index = 0; index < _objects.Length; index++)
            {
                var obj = _objects[index];
                obj.SetActive(ActiveObj);
            }
        }
    }
}
