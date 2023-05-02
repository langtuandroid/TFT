
namespace Attack
{
    public interface IAttack
    {
        /// <summary>
        /// Ejecuta ataques
        /// </summary>
        void Execute(bool pressed);

        /// <summary>
        /// Ataque débil
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
        /// Método para indicar que se pasa a ataque fuerte
        /// </summary>
        void ChangeStrongAttackState();

        /// <summary>
        /// Resetea los valores de las variables que contiene
        /// </summary>
        void ResetValues();
    }

}
