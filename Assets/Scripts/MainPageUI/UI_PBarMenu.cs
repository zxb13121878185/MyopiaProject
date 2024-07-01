using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PBarMenu : GlobalBaseObj
{
    public UI_BtnEffect btnLoginOut;
    protected override void FixedUpdate_State()
    {

    }

    protected override void LateUpdate_State()
    {

    }

    protected override void Update_State()
    {

    }

    public override void Init(bool isActive = true)
    {
        base.Init(isActive);
        btnLoginOut.gameObject.SetActive(!Global_Manage.M_Instance.M_IsLocalVersion);
    }
}
