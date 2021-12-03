using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// Peut s'enregistrer dans un autre scriptableObjet (comme pour les coroutines)
[CreateAssetMenu(menuName ="Variables/Text")]
public class TextVariable : GenericVariable<TMP_Text> { }
