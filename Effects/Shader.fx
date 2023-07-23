float l;


float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float l2 = 2.0 * l - 1.0;
    //return float4(l2,l2,l2,1);
    float l3 = sqrt(l2 * l2); //replace with l3 = sqrt(l2 * l2) for it to work
    return float4(l3,l3,l3,1);    
}


technique Shader
{
    pass PixelShaderFunction
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}