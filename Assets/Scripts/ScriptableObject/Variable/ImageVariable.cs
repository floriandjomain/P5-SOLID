using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// Peut s'enregistrer dans un autre scriptableObjet (comme pour les coroutines)
[CreateAssetMenu(menuName = "Variables/Image")]
public class ImageVariable : GenericVariable<Image> { }
