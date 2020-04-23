using UnityEngine;


namespace DefaultNamespace
{
    public sealed class PlayerHP : MonoBehaviour, IInteractable
    {
        [SerializeField] private float _hp;

        private float _hpMax = 100f;
        private float _hpMin = 0f;

        public void SetValue(float value)
        {
            _hp += value;

            if (_hp > _hpMax)
            {
                _hp = _hpMax;
            }

            if (_hp <= _hpMin)
            {
                _hp = _hpMin;
                // Вызов метода смерти игрока
            }
        }
    }
}
