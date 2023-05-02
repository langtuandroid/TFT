
namespace Attack
{
    public interface IAttack
    {
        /// <summary>
        /// Ejecuta ataques
        /// </summary>
        void Execute(bool pressed);

        /// <summary>
        /// Ataque d�bil
        /// </summary>
        void WeakAttack();

        /// <summary>
        /// Ataque medio
        /// </summary>
        void MediumAttack();

        /// <summary>
        /// Ataque fuerte
        /// </summary>
        void StrongAttack();

        /// <summary>
        /// M�todo para indicar que se pasa a ataque fuerte
        /// </summary>
        void ChangeStrongAttackState();

        /// <summary>
        /// Resetea los valores de las variables que contiene
        /// </summary>
        void ResetValues();
    }

}
