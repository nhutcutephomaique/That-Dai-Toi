# THẤT TỘI — Vietnamese Psychological Horror

## 1. Giới thiệu dự án
- **Tên game:** Thất Tội
- **Thể loại:** Kinh dị tâm lý, giải đố, khám phá
- **Góc nhìn:** First-person
- **Engine:** Unity 3D
- **Nền tảng mục tiêu:** PC
- **Quy mô:** Solo Indie
- **Bối cảnh:** Một con hẻm nhỏ tại Việt Nam
- **Chủ đề:** Thất Đại Tội, ký ức, tội lỗi và sự thật bị che giấu.

## 2. Ý tưởng cốt lõi
- Xây dựng trải nghiệm kinh dị từ môi trường đời thường Việt Nam.
- Khám phá một con hẻm chứa những bí mật liên quan đến một bi kịch trong quá khứ.
- Sử dụng âm thanh, ánh sáng, vật thể và hiện tượng siêu nhiên để tạo cảm giác bất an.
- Lồng ghép bảy tội lỗi vào cốt truyện, câu đố và hành vi của các nhân vật.
- Ưu tiên chất lượng không khí và kể chuyện thay vì quy mô bản đồ lớn.

## 3. Cốt truyện
- Nhân vật chính quay trở lại con hẻm cũ.
- Những sự kiện kỳ lạ bắt đầu xuất hiện.
- Người chơi tìm kiếm manh mối thông qua đồ vật, ghi âm và ký ức.
- Các bí mật về bi kịch trong quá khứ dần được hé lộ.
- Lựa chọn và cách người chơi khám phá dẫn đến những kết thúc khác nhau.

## 4. Gameplay
- Di chuyển và khám phá môi trường.
- Tương tác với đồ vật.
- Thu thập vật phẩm và manh mối.
- Giải đố dựa trên môi trường.
- Đọc ghi chú và nghe bản ghi âm.
- Phát hiện các hiện tượng siêu nhiên.
- Trốn tránh hoặc sống sót trước mối đe dọa.
- Mở khóa khu vực và sự kiện mới.

## 5. Phạm vi dự án

### Bản đồ
- Một con hẻm chính.
- Quán trà đá.
- Một số mặt tiền nhà dân.
- Một cửa hàng nhỏ.
- Một căn nhà trọng tâm có nội thất.
- Một đoạn cuối hẻm dùng cho sự kiện kinh dị.

### Nội dung
- Bảy tội lỗi liên kết trong cùng một cốt truyện.
- Các câu đố và manh mối.
- Những sự kiện kinh dị theo kịch bản.
- Nhiều kết thúc dựa trên nội dung khám phá.

### Giới hạn phát triển
- Không xây dựng thành phố hoặc bản đồ thế giới mở.
- Không tự tạo toàn bộ mô hình 3D từ đầu.
- Tận dụng asset có sẵn và môi trường modular.
- Tập trung vào một số ít khu vực có chất lượng cao.

## 6. Công nghệ và công cụ
- Unity 3D.
- C#.
- Git và GitHub.
- Blender khi cần chỉnh sửa mô hình đơn giản.
- Asset Store và các thư viện asset có giấy phép phù hợp.
- Công cụ xử lý âm thanh và tạo hiệu ứng hậu kỳ.

## 7. Kiến trúc hệ thống dự kiến
- `PlayerController`: Điều khiển nhân vật.
- `PlayerInteraction`: Tương tác với đồ vật.
- `InteractionObject`: Các vật thể có thể tương tác.
- `InventorySystem`: Quản lý vật phẩm.
- `PuzzleSystem`: Xử lý câu đố.
- `DialogueSystem`: Hội thoại và ghi chú.
- `AudioManager`: Âm thanh và nhạc nền.
- `EventManager`: Kích hoạt sự kiện kinh dị.
- `GameManager`: Quản lý trạng thái trò chơi.
- `SaveSystem`: Lưu tiến trình.
- `SceneLoader`: Chuyển cảnh.

*Các hệ thống trên là kế hoạch dự kiến, chưa được xem là đã triển khai.*

## 8. Kế hoạch phát triển

### Giai đoạn 1 — Prototype
- [ ] Tạo project Unity và cấu hình Git.
- [ ] Thiết lập điều khiển góc nhìn thứ nhất.
- [ ] Dựng con hẻm bằng hình khối đơn giản.
- [ ] Tạo hệ thống tương tác cơ bản.
- [ ] Thêm một câu đố.
- [ ] Tạo một sự kiện ma xuất hiện.
- [ ] Hoàn thành một đoạn chơi ngắn.

### Giai đoạn 2 — Vertical Slice
- [ ] Hoàn thiện hình ảnh một khu vực.
- [ ] Thêm âm thanh môi trường và ánh sáng.
- [ ] Hoàn thiện một chuỗi câu đố.
- [ ] Thêm cốt truyện và manh mối.
- [ ] Tạo một đoạn kết ngắn có thể chơi được.

### Giai đoạn 3 — Phát triển nội dung
- [ ] Mở rộng cốt truyện cho đủ bảy tội lỗi.
- [ ] Hoàn thiện các khu vực và sự kiện.
- [ ] Thêm các kết thúc.
- [ ] Tối ưu hiệu năng.
- [ ] Kiểm thử toàn bộ trải nghiệm.

### Giai đoạn 4 — Phát hành
- [ ] Sửa lỗi.
- [ ] Hoàn thiện giao diện.
- [ ] Thêm tùy chỉnh âm thanh và đồ họa cơ bản.
- [ ] Xây dựng bản chơi thử.
- [ ] Đóng gói bản phát hành PC.

## 9. Cấu trúc thư mục dự kiến

```text
Assets/
├── Art/
│   ├── Materials/
│   ├── Models/
│   ├── Textures/
│   └── Lighting/
├── Audio/
│   ├── Ambient/
│   ├── SFX/
│   └── Music/
├── Prefabs/
│   ├── Environment/
│   ├── Interactables/
│   └── Events/
├── Scenes/
├── Scripts/
│   ├── Player/
│   ├── Interaction/
│   ├── Puzzle/
│   ├── Audio/
│   ├── Events/
│   └── Core/
├── ScriptableObjects/
└── UI/
```

## 10. Quy tắc quản lý Git
- `main`: Phiên bản ổn định.
- `develop`: Tích hợp các tính năng đang phát triển.
- `feature/*`: Phát triển từng tính năng.
- `fix/*`: Sửa lỗi.
- Commit theo từng thay đổi nhỏ, có ý nghĩa.
- Không commit thư mục `Library/`, `Temp/`, `Obj/` và các file sinh tự động.
- Sử dụng Git LFS nếu cần quản lý asset dung lượng lớn.
- Không đưa asset bên thứ ba lên GitHub nếu giấy phép không cho phép.

## 11. Tiêu chí hoàn thành
- Người chơi có thể đi hết khu vực được thiết kế.
- Hệ thống tương tác hoạt động ổn định.
- Câu đố có thể giải được và có phản hồi rõ ràng.
- Sự kiện kinh dị được kích hoạt đúng điều kiện.
- Cốt truyện có thể hiểu được thông qua manh mối.
- Không có lỗi nghiêm trọng ngăn cản tiến trình.
- Có bản build PC chạy độc lập.

## 12. Tài nguyên tham khảo
- Unity Asset Store: https://assetstore.unity.com/
- Poly Haven: https://polyhaven.com/
- ambientCG: https://ambientcg.com/
- Sketchfab: https://sketchfab.com/

**Lưu ý:** Kiểm tra giấy phép của từng asset trước khi sử dụng hoặc phân phối game.

## 13. Trạng thái dự án
- **Giai đoạn hiện tại:** Lên kế hoạch.
- **Phiên bản:** 0.1.0 — Prototype Planning.
- **Mục tiêu trước mắt:** Hoàn thành một đoạn chơi ngắn trong con hẻm, có khám phá, giải đố và một sự kiện kinh dị hoàn chỉnh.
## 14. Hướng dẫn chơi:
- Di chuyển: WASD
- Nhặt vật phẩm: Chuột trái
- Chạy: shift trái

