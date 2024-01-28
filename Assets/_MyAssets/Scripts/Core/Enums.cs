
/// <summary>
/// 一括でロードしたアセットを取得する際のキー
/// ロードするアセットが増えるに応じて追加される
/// </summary>
public enum AssetKey
{
    Dummy,
    Bowl, // 掴めるどんぶり
    SoupParticle, // お玉から発生するパーティクル
    CharSiu, // 掴めるチャーシュー
    CharSiuModel, // どんぶり内に表示されるチャーシュー
    NoodleModel, // どんぶり内に表示される麺
    SoupModel, // どんぶり内に表示されるスープ
    CollisionSoup, // お玉から流れるスープの判定
    NoodleObject, // 湯切りざるから出る判定とレンダラーを持った麺
    Renge, // 掴むことが出来るレンゲ
    WaterDrop, // 湯切りざるを振った際に飛び散る水滴
    DrainWater, // 湯切りざるを持ち上げた際に排出される水
    BowlShitazara, // ボウルの下皿
    Kikurage, // きくらげ
}

/// <summary>
/// 具材を扱う際の方法
/// </summary>
public enum HandleType
{
    LadleScoop, // お玉ですくう
    HandGrab, // 手づかみ
    TongsGrab, // トングでつかむ
}

/// <summary>
/// オブジェクトやデータなどで食材の種類を判定する
/// </summary>
public enum FoodType
{
    Soup,
    Noodle,
    CharSiu,
    Renge,
    Kikurage,
    Dummy, // ダミー
}