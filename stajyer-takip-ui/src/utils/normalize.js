export function normalizeIntern(dto = {}) {
  return {
    id: dto.id ?? dto.internId ?? dto.guid ?? dto._id,

    firstName: dto.firstName ?? dto.ad ?? "",
    lastName:  dto.lastName  ?? dto.soyad ?? "",

    identityNumber:
      dto.identityNumber ?? dto.tcKimlikNo ?? dto.tckn ?? dto.tc ?? "",

    email: dto.email ?? "",
    phone: dto.phone ?? dto.telefon ?? "",

    school: dto.school ?? dto.okul ?? "",
    department: dto.department ?? dto.bolum ?? dto["bölüm"] ?? "",

    startDate: dto.startDate ?? dto.baslangicTarihi ?? dto.baslangic ?? "",
    endDate:   dto.endDate   ?? dto.bitisTarihi     ?? dto.bitis    ?? "",

    status: dto.status ?? dto.durum ?? "",
  };
}
