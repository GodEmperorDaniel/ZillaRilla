using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Enemy.Finite_State_Machines.States
{
    [CreateAssetMenu(fileName = "DeathState", menuName = "ZillaRilla/States/Death", order = 3)]
    public class DeathState : AbstractFSMState
    {
        private const string cNewsCategory = "Enemy Killed";
        
        [SerializeField] private List<GameObject> _bugSplatDecal;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private List<ParticleSystem> _particalList;
        private ZillaAttacks _zilla;
        private RillaAttacks _rilla;


        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.DEATH;
        }
        
        public override bool EnterState()
        {
            //GameManager.Instance._zilla.gameObject.TryGetComponent<ZillaAttacks>(out _zilla); 
            //GameManager.Instance._rilla.gameObject.TryGetComponent<RillaAttacks>(out _rilla);
            _navMeshAgent.isStopped = true;
            EnteredState = base.EnterState();

            if (EnteredState)
            {

                //Debug.Log("ENTERED DEATH STATE");
                //_zilla.RemoveFromPlayerList(_fsm.gameObject); //think these can be removed now :p
                //_rilla.RemoveFromPlayerList(_fsm.gameObject);
                GameObject randomDecal = _bugSplatDecal[Randomizer(0, _bugSplatDecal.Count - 1)];
                //Debug.Log(_npc.transform.rotation);
                Instantiate(randomDecal, _fsm.gameObject.transform.position + _offset, Quaternion.Euler(90, _npc.transform.rotation.y * Mathf.Rad2Deg, _npc.transform.rotation.z * Mathf.Rad2Deg));
                foreach (ParticleSystem partical in _particalList)
                {
                    Instantiate(partical, _fsm.gameObject.transform.position + new Vector3(0, 8, 0), _npc.PlayerTransform.rotation);
                }
                
                Destroy(_npc.gameObject, _npc.deSpawnTime);
            }
            return EnteredState;

        }

        public override void UpdateState()
        {
            //Debug.Log("UPDATING DEATH STATE");
        }
        /// <summary>
        /// Is inclusive!!
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private int Randomizer(float min, float max)
        {
            return (int)UnityEngine.Random.Range(min,max);
        }
    }
}
