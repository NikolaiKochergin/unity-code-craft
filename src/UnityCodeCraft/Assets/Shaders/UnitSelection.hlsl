#define PI 3.14159265359

half4 DrawSelectionCircle(
    float3 position,
    float3 center,
    const float radius,
    const float thickness,
    half4 color
)
{
    const float2 delta = position.xz - center.xz;
    const float distance = length(delta);
    const float ring =
        smoothstep(radius + thickness, radius + thickness * 0.5, distance) *
        smoothstep(radius, radius + thickness * 0.5, distance);

    return half4(color.rgb, color.a * ring);
}

half4 DrawHoverCircle(
    float3 position,
    float3 center,
    const float radius,
    const float thickness,
    half4 color,
    const float dash,
    const float speed
)
{
    color = DrawSelectionCircle(position, center, radius, thickness, color);
    
    // Угол в радианах [-π, π]
    const float2 delta = position.xz - center.xz;
    const float angle = atan2(delta.y, delta.x);

    // Нормализованный угол [0,1]
    float normalizedAngle = frac(angle / (2.0 * PI));

    // Вращение по времени
    const float offset = _Time.y * speed; // используем глоб. время
    normalizedAngle = frac(normalizedAngle + offset);

    // Пунктир
    const float dashFactor = step(0.5, frac(normalizedAngle * dash * 2.0));
    return color * dashFactor;
}
