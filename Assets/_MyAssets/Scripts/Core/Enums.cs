
/// <summary>
/// �ꊇ�Ń��[�h�����A�Z�b�g���擾����ۂ̃L�[
/// ���[�h����A�Z�b�g��������ɉ����Ēǉ������
/// </summary>
public enum AssetKey
{
    Dummy,
    Bowl, // �͂߂�ǂ�Ԃ�
    SoupParticle, // ���ʂ��甭������p�[�e�B�N��
    CharSiu, // �͂߂�`���[�V���[
    CharSiuModel, // �ǂ�Ԃ���ɕ\�������`���[�V���[
    NoodleModel, // �ǂ�Ԃ���ɕ\��������
    SoupModel, // �ǂ�Ԃ���ɕ\�������X�[�v
    CollisionSoup, // ���ʂ��痬���X�[�v�̔���
    NoodleObject, // ���؂肴�邩��o�锻��ƃ����_���[����������
    Renge, // �͂ނ��Ƃ��o���郌���Q
    WaterDrop, // ���؂肴���U�����ۂɔ�юU�鐅�H
    DrainWater, // ���؂肴��������グ���ۂɔr�o����鐅
    BowlShitazara, // �{�E���̉��M
    Kikurage, // �����炰
}

/// <summary>
/// ��ނ������ۂ̕��@
/// </summary>
public enum HandleType
{
    LadleScoop, // ���ʂł�����
    HandGrab, // ��Â���
    TongsGrab, // �g���O�ł���
}

/// <summary>
/// �I�u�W�F�N�g��f�[�^�ȂǂŐH�ނ̎�ނ𔻒肷��
/// </summary>
public enum FoodType
{
    Soup,
    Noodle,
    CharSiu,
    Renge,
    Kikurage,
    Dummy, // �_�~�[
}