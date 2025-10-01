package crc644b99aa05fc0faf94;


public class PlatformTextField
	extends android.widget.TextView
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getDefaultEditable:()Z:GetGetDefaultEditableHandler\n" +
			"n_getFreezesText:()Z:GetGetFreezesTextHandler\n" +
			"n_setFreezesText:(Z)V:GetSetFreezesText_ZHandler\n" +
			"n_getDefaultMovementMethod:()Landroid/text/method/MovementMethod;:GetGetDefaultMovementMethodHandler\n" +
			"n_getSelectionStart:()I:GetGetSelectionStartHandler\n" +
			"n_getSelectionEnd:()I:GetGetSelectionEndHandler\n" +
			"n_onTextChanged:(Ljava/lang/CharSequence;III)V:GetOnTextChanged_Ljava_lang_CharSequence_IIIHandler\n" +
			"n_onSelectionChanged:(II)V:GetOnSelectionChanged_IIHandler\n" +
			"n_onCheckIsTextEditor:()Z:GetOnCheckIsTextEditorHandler\n" +
			"n_setText:(Ljava/lang/CharSequence;Landroid/widget/TextView$BufferType;)V:GetSetText_Ljava_lang_CharSequence_Landroid_widget_TextView_BufferType_Handler\n" +
			"n_draw:(Landroid/graphics/Canvas;)V:GetDraw_Landroid_graphics_Canvas_Handler\n" +
			"n_onSizeChanged:(IIII)V:GetOnSizeChanged_IIIIHandler\n" +
			"n_onLayout:(ZIIII)V:GetOnLayout_ZIIIIHandler\n" +
			"n_onTouchEvent:(Landroid/view/MotionEvent;)Z:GetOnTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"n_onHoverEvent:(Landroid/view/MotionEvent;)Z:GetOnHoverEvent_Landroid_view_MotionEvent_Handler\n" +
			"";
		mono.android.Runtime.register ("Material.Components.Maui.Platform.PlatformTextField, Material.Components.Maui", PlatformTextField.class, __md_methods);
	}

	public PlatformTextField (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == PlatformTextField.class) {
			mono.android.TypeManager.Activate ("Material.Components.Maui.Platform.PlatformTextField, Material.Components.Maui", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, System.Private.CoreLib:System.Int32, System.Private.CoreLib", this, new java.lang.Object[] { p0, p1, p2, p3 });
		}
	}

	public PlatformTextField (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == PlatformTextField.class) {
			mono.android.TypeManager.Activate ("Material.Components.Maui.Platform.PlatformTextField, Material.Components.Maui", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, System.Private.CoreLib", this, new java.lang.Object[] { p0, p1, p2 });
		}
	}

	public PlatformTextField (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == PlatformTextField.class) {
			mono.android.TypeManager.Activate ("Material.Components.Maui.Platform.PlatformTextField, Material.Components.Maui", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
		}
	}

	public PlatformTextField (android.content.Context p0)
	{
		super (p0);
		if (getClass () == PlatformTextField.class) {
			mono.android.TypeManager.Activate ("Material.Components.Maui.Platform.PlatformTextField, Material.Components.Maui", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}

	public boolean getDefaultEditable ()
	{
		return n_getDefaultEditable ();
	}

	private native boolean n_getDefaultEditable ();

	public boolean getFreezesText ()
	{
		return n_getFreezesText ();
	}

	private native boolean n_getFreezesText ();

	public void setFreezesText (boolean p0)
	{
		n_setFreezesText (p0);
	}

	private native void n_setFreezesText (boolean p0);

	public android.text.method.MovementMethod getDefaultMovementMethod ()
	{
		return n_getDefaultMovementMethod ();
	}

	private native android.text.method.MovementMethod n_getDefaultMovementMethod ();

	public int getSelectionStart ()
	{
		return n_getSelectionStart ();
	}

	private native int n_getSelectionStart ();

	public int getSelectionEnd ()
	{
		return n_getSelectionEnd ();
	}

	private native int n_getSelectionEnd ();

	public void onTextChanged (java.lang.CharSequence p0, int p1, int p2, int p3)
	{
		n_onTextChanged (p0, p1, p2, p3);
	}

	private native void n_onTextChanged (java.lang.CharSequence p0, int p1, int p2, int p3);

	public void onSelectionChanged (int p0, int p1)
	{
		n_onSelectionChanged (p0, p1);
	}

	private native void n_onSelectionChanged (int p0, int p1);

	public boolean onCheckIsTextEditor ()
	{
		return n_onCheckIsTextEditor ();
	}

	private native boolean n_onCheckIsTextEditor ();

	public void setText (java.lang.CharSequence p0, android.widget.TextView.BufferType p1)
	{
		n_setText (p0, p1);
	}

	private native void n_setText (java.lang.CharSequence p0, android.widget.TextView.BufferType p1);

	public void draw (android.graphics.Canvas p0)
	{
		n_draw (p0);
	}

	private native void n_draw (android.graphics.Canvas p0);

	public void onSizeChanged (int p0, int p1, int p2, int p3)
	{
		n_onSizeChanged (p0, p1, p2, p3);
	}

	private native void n_onSizeChanged (int p0, int p1, int p2, int p3);

	public void onLayout (boolean p0, int p1, int p2, int p3, int p4)
	{
		n_onLayout (p0, p1, p2, p3, p4);
	}

	private native void n_onLayout (boolean p0, int p1, int p2, int p3, int p4);

	public boolean onTouchEvent (android.view.MotionEvent p0)
	{
		return n_onTouchEvent (p0);
	}

	private native boolean n_onTouchEvent (android.view.MotionEvent p0);

	public boolean onHoverEvent (android.view.MotionEvent p0)
	{
		return n_onHoverEvent (p0);
	}

	private native boolean n_onHoverEvent (android.view.MotionEvent p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
