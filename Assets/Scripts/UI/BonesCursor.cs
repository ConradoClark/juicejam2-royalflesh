using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.UI;
using UnityEngine;

public class BonesCursor : UICursor
{
    public Color BlinkColor;
    public SpriteRenderer Cursor;
    public UIMenuContext MenuContext;
    private bool _blink;
    private bool _enabled;

    private void OnEnable()
    {
        MenuContext.OnCursorMoved += MenuContext_OnCursorMoved;
        _enabled = true;
        DefaultMachinery.AddBasicMachine(HandleBlink());
    }

    private void OnDisable()
    {
        _enabled = false;
    }

    private void MenuContext_OnCursorMoved(UIAction obj)
    {
        _blink = obj is SelectBonesUIAction act && act.HasMoreEquipment();
    }

    private IEnumerable<IEnumerable<Action>> HandleBlink()
    {
        while (_enabled)
        {
            while (_blink)
            {
                Cursor.color = BlinkColor;
                yield return TimeYields.WaitMilliseconds(UITimer, 100);
                Cursor.color = Color.white;
                yield return TimeYields.WaitMilliseconds(UITimer, 100);
            }

            Cursor.color = Color.white;

            yield return TimeYields.WaitOneFrameX;
        }
    }
}
