using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewAlchemyRecipe", menuName = "Alchemy/Alchemy Recipe")]
public class AlchemyRecipe : ScriptableObject
{
    [Tooltip("Lista ID przedmiotów, które s¹ potrzebne do stworzenia roztworu.")]
    public List<int> ingredientItemIds;

    [Tooltip("Prefab przedmiotu, który powstaje po uwarzeniu eliksiru (np. 'Fiolka z Eliksirem Si³y').")]
    public InteractableItem resultingPotion;

    public Material recipeMat;

    public float brewingTime = 5f;
}