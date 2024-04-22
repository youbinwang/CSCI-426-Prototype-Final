void IsColorClose_float(float3 sampleColor, float colorIndex, float threshold, out float3 result){
    float3 colors[9] = {
        float3(178 / 255.0, 143 / 255.0, 141 / 255.0), // red
        float3(154 / 255.0, 109 / 255.0, 104 / 255.0), // darkred
        float3(128 / 255.0, 100 / 255.0, 141 / 255.0), // darkpurple
        float3(158 / 255.0, 140 / 255.0, 164 / 255.0), // lightpurple
        float3(129 / 255.0, 165 / 255.0, 159 / 255.0), // lightgreen
        float3(97 / 255.0, 153 / 255.0, 142 / 255.0),  // darkgreen
        float3(133 / 255.0, 163 / 255.0, 146 / 255.0), // green
        float3(178 / 255.0, 168 / 255.0, 128 / 255.0), // yellow
        float3(160 / 255.0, 116 / 255.0, 89 / 255.0)   // orange3
    };

    // Ensure colorIndex is within array bounds
    colorIndex = clamp(colorIndex, 0, 8);

    // Check if the sampleColor is within the threshold distance of the target color
	int _c = round(colorIndex);
    float3 colorDiff = abs(sampleColor - colors[_c]);
    result = all(colorDiff < threshold);
    //result = colorDiff < threshold ? float3(1, 1, 1) : float3(0, 0, 0) ;
    //result = all(colorDiff < threshold) ? 1.0 : 0.0;
}
