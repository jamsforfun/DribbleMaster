#ifndef BLEND_MODES_CGINC
#define BLEND_MODES_CGINC

fixed4 Darken(fixed4 a, fixed4 b)
{
	fixed4 r = min(a, b);
	r.a = b.a;
	return r;
}

fixed4 Lighten(fixed4 a, fixed4 b)
{
	fixed4 r = max(a, b);
	r.a = b.a;
	return r;
}

fixed4 Overlay(fixed4 a, fixed4 b)
{
	fixed4 r = a < .5 ? 2.0 * a * b : 1.0 - 2.0 * (1.0 - a) * (1.0 - b);
	r.a = b.a;
	return r;
}

fixed4 Screen(fixed4 a, fixed4 b)
{
	fixed4 r = 1.0 - (1.0 - a) * (1.0 - b);
	r.a = b.a;
	return r;
}

fixed4 ColorDodge(fixed4 a, fixed4 b)
{
	fixed4 r = a / (1 - b);
	r.a = b.a;
	return r;
}

fixed4 Multiply(fixed4 a, fixed4 b)
{
	fixed4 r = a * b;
	r.a = b.a;
	return r;
}

fixed4 ColorBurn(fixed4 a, fixed4 b)
{
	fixed4 r = 1 - (1 - b) / a;
	r.a = b.a;
	return r;
}

fixed4 LinearBurn(fixed4 a, fixed4 b)
{
	fixed4 r = a + b - 1;
	r.a = b.a;
	return r;
}

fixed4 Exclusion(fixed4 a, fixed4 b)
{
	fixed4 r = 0.5 - 2 * (a - 0.5)*(b - 0.5);
	r.a = b.a;
	return r;
}

#endif