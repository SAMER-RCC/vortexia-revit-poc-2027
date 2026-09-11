# 📝 إرشادات المساهمة

شكرًا لاهتمامك بالمساهمة في **Vortexia Revit POC 2027**! 🎉

نرحب بجميع أنواع المساهمات:
- 🐛 تقارير الأخطاء
- ✨ الميزات الجديدة
- 📚 تحسينات التوثيق
- 🧪 الاختبارات
- 🚀 تحسينات الأداء

---

## 📋 قبل أن تبدأ

### اقرأ أولاً:
1. [README.md](../README.md) - نظرة عامة على المشروع
2. [ARCHITECTURE.md](docs/ARCHITECTURE.md) - الهندسة المعمارية
3. [GETTING_STARTED.md](docs/GETTING_STARTED.md) - دليل البدء
4. [PROJECT_ROADMAP.md](../PROJECT_ROADMAP.md) - خطة العمل

### تحقق من:
- [ ] هل تم حل المشكلة من قبل؟ (اقرأ Issues والـ Discussions)
- [ ] هل ترتبط المساهمة بالخطة الموضوعة؟
- [ ] هل لديك البيئة المطلوبة مثبتة؟

---

## 🚀 خطوات المساهمة

### 1️⃣ Fork المستودع

```bash
# في GitHub، اضغط زر Fork
# ثم استنسخ نسختك المحلية
git clone https://github.com/YOUR_USERNAME/vortexia-revit-poc-2027.git
cd vortexia-revit-poc-2027
```

### 2️⃣ إنشاء فرع للعمل

```bash
# اختر اسم واضح وموصوف
git checkout -b feature/your-feature-name
# أو للأخطاء:
git checkout -b fix/your-bug-fix
# أو للتوثيق:
git checkout -b docs/your-documentation-update
```

### 3️⃣ قم بالتطوير

```bash
# تأكد من الاتصال بـ upstream
git remote add upstream https://github.com/SAMER-RCC/vortexia-revit-poc-2027.git

# حدث بيانات المستودع الأساسي
git fetch upstream
git rebase upstream/main

# اعمل على ميزتك
# ...اكتب الكود...
```

### 4️⃣ الاختبار والتحقق

```bash
# قم بتشغيل جميع الاختبارات
dotnet test

# تحقق من تنسيق الكود
dotnet format

# تشغيل analyzer
dotnet build --no-incremental /p:TreatWarningsAsErrors=true
```

### 5️⃣ Commit التغييرات

```bash
# اتبع معيار Git Conventional Commits
git commit -m "type(scope): description"

# الأنواع المسموحة:
# feat:     ميزة جديدة
# fix:      إصلاح خطأ
# docs:     تحديث التوثيق
# style:    تنسيق الكود (لا يؤثر على الكود)
# refactor: تعديل الكود بدون تغيير الوظيفة
# perf:     تحسين الأداء
# test:     إضافة أو تحديث الاختبارات
# chore:    تحديثات أخرى (مثل dependencies)

# أمثلة:
git commit -m "feat(commands): add new CreateWallCommand"
git commit -m "fix(ui): resolve null reference exception"
git commit -m "docs: update getting started guide"
```

### 6️⃣ Push الفرع

```bash
# ارفع الفرع إلى نسختك
git push origin feature/your-feature-name
```

### 7️⃣ افتح Pull Request

1. اذهب إلى مستودع GitHub الأصلي
2. اضغط **"Compare & pull request"**
3. ملء التفاصيل:
   - **العنوان:** موجز واضح للتغييرات
   - **الوصف:** شرح مفصل (استخدم القالب بالأسفل)
   - **الفئات:** أضف labels (bug, feature, documentation, etc.)
   - **الـ Milestone:** اختر الإصدار المناسب

---

## 📋 قالب Pull Request

```markdown
## الوصف
اشرح ما تفعله هذه PR بوضوح وموجز.

### نوع التغيير
- [ ] 🐛 إصلاح خطأ (bug fix)
- [ ] ✨ ميزة جديدة (new feature)
- [ ] 📚 تحديث التوثيق (documentation)
- [ ] 🔄 إعادة هيكلة الكود (refactoring)
- [ ] ⚡ تحسين الأداء (performance)

### المشاكل المرتبطة
الإغلاق: #(issue_number)

### قائمة التحقق
- [ ] قرأت [CONTRIBUTION.md](CONTRIBUTION.md)
- [ ] تم تحديث التوثيق إن لزم
- [ ] أضفت اختبارات جديدة (إن وجد)
- [ ] اجتازت جميع الاختبارات المحلية
- [ ] الكود يتبع معايير المشروع
- [ ] لا توجد رسائل خطأ في console

### لقطات شاشة (إن لزم)
أضف صور توضيحية للتغييرات البصرية.

### ملاحظات إضافية
أي شيء آخر يريد أن تشاركه مع المراجعين.
```

---

## 📋 قالب تقرير الخطأ (Bug Report)

```markdown
## الوصف
اشرح الخطأ بوضوح.

## الخطوات لتكرار الخطأ
1. افعل كذا
2. ثم افعل كذا
3. ستحصل على الخطأ التالي

## السلوك المتوقع
ماذا كان يجب أن يحدث.

## السلوك الفعلي
ماذا حدث بالفعل.

## البيئة
- **النظام:** Windows 10/11
- **Visual Studio:** 2022 (Version X.X.X)
- **.NET Version:** 8.0
- **Revit Version:** 2025
- **Branch:** main/develop

## رسالة الخطأ / Stack Trace
```
Copy/paste الخطأ هنا
```

## ملفات إضافية
أرفق أي ملفات تساعد في فهم المشكلة.

## ملاحظات إضافية
أي معلومات أخرى مهمة.
```

---

## 📋 قالب اقتراح ميزة جديدة (Feature Request)

```markdown
## المشكلة
اشرح المشكلة التي تحل.

## الحل المقترح
اشرح حلك.

## بدائل أخرى
هل فكرت في حلول أخرى؟

## سياق إضافي
معلومات أخرى قد تكون مفيدة.

## الأولوية
- [ ] حرجة (Critical)
- [ ] عالية (High)
- [ ] متوسطة (Medium)
- [ ] منخفضة (Low)
```

---

## 🎨 معايير الكود

### نمط التسمية

```csharp
// الفئات (Classes): PascalCase
public class MasterController { }

// الدوال (Methods): PascalCase
public async Task ExecuteCommandAsync() { }

// المتغيرات المحلية: camelCase
var commandManager = new CommandManager();

// الثوابت: UPPER_SNAKE_CASE
private const string DEFAULT_TEMPLATE = "StandardWall";

// الخصائص: PascalCase
public string Name { get; set; }

// الـ Interfaces: ابدأ بـ I
public interface ICommand { }

// الـ Private fields: _camelCase
private readonly ICommandManager _commandManager;
```

### معايير الكود

1. **SOLID Principles:**
   ```csharp
   // ✅ جيد - Single Responsibility
   public class CommandExecutor
   {
       public async Task ExecuteAsync(ICommand command) { }
   }

   // ❌ سيء - مسؤوليات متعددة
   public class SuperClass
   {
       public void Execute() { }
       public void Log() { }
       public void Save() { }
   }
   ```

2. **Dependency Injection:**
   ```csharp
   // ✅ جيد
   public class CommandManager
   {
       private readonly ILogger<CommandManager> _logger;
       
       public CommandManager(ILogger<CommandManager> logger)
       {
           _logger = logger;
       }
   }

   // ❌ سيء
   public class CommandManager
   {
       private readonly ILogger _logger = new Logger(); // Tight coupling
   }
   ```

3. **Async/Await:**
   ```csharp
   // ✅ جيد
   public async Task<CommandResult> ExecuteAsync()
   {
       var result = await ProcessAsync();
       return result;
   }

   // ❌ سيء
   public void Execute()
   {
       ProcessAsync().Wait(); // Blocking
   }
   ```

4. **Exception Handling:**
   ```csharp
   // ✅ جيد
   try
   {
       await ExecuteAsync();
   }
   catch (InvalidOperationException ex)
   {
       _logger.LogError(ex, "Invalid operation");
       // Handle specific exception
   }
   catch (Exception ex)
   {
       _logger.LogError(ex, "Unexpected error");
       throw;
   }

   // ❌ سيء
   try { } catch { } // Silent failures
   ```

5. **Documentation:**
   ```csharp
   // ✅ جيد
   /// <summary>
   /// Executes a command asynchronously.
   /// </summary>
   /// <param name="command">The command to execute.</param>
   /// <returns>The result of the command execution.</returns>
   /// <exception cref="ArgumentNullException">Thrown when command is null.</exception>
   public async Task<CommandResult> ExecuteAsync(ICommand command)
   {
   }

   // ❌ سيء
   public async Task<CommandResult> ExecuteAsync(ICommand command)
   {
   }
   ```

---

## 🧪 متطلبات الاختبارات

1. **كتابة Unit Tests:**
   ```csharp
   [TestClass]
   public class CommandManagerTests
   {
       [TestMethod]
       public async Task ExecuteAsync_WithValidCommand_ReturnsSuccess()
       {
           // Arrange
           var manager = new CommandManager();
           var command = new TestCommand();

           // Act
           var result = await manager.ExecuteAsync(command);

           // Assert
           Assert.AreEqual(CommandStatus.Success, result.Status);
       }
   }
   ```

2. **تغطية الاختبارات:**
   - الحد الأدنى: 70%
   - المثالي: 85%+

3. **أسماء الاختبارات:**
   ```
   MethodName_Condition_ExpectedResult
   
   ✅ ExecuteAsync_WithNullCommand_ThrowsArgumentNullException
   ❌ Test1, TestMethod, etc.
   ```

---

## 📚 معايير التوثيق

### ملفات Markdown

1. استخدم **Heading Hierarchy** الصحيح (# → ## → ###)
2. أضف **Table of Contents** للملفات الطويلة
3. استخدم **Code Blocks** مع اللغة المناسبة
4. أضف **Links** للملفات الأخرى عند الحاجة
5. استخدم **Lists** ورموز emoji للوضوح

### XML Documentation

```csharp
/// <summary>
/// Brief description (one line).
/// </summary>
/// <remarks>
/// Additional details and examples.
/// </remarks>
/// <param name="parameter1">Description of parameter1.</param>
/// <param name="parameter2">Description of parameter2.</param>
/// <returns>Description of return value.</returns>
/// <exception cref="ExceptionType">When this exception is thrown.</exception>
/// <example>
/// <code>
/// // Example usage
/// var result = await Method(param1, param2);
/// </code>
/// </example>
public async Task<T> Method(string parameter1, int parameter2)
{
    // Implementation
}
```

---

## 🔄 عملية المراجعة

1. **CI/CD Checks:**
   - ✅ Build Successful
   - ✅ All Tests Pass
   - ✅ Code Analysis Pass
   - ✅ Documentation Updated

2. **Code Review:**
   - تفاعل مع الآراء بشكل احترافي
   - أضف commits للتحديثات (لا تعيد كتابة الـ history)
   - اطلب توضيحات عند الحاجة

3. **Approval:**
   - يحتاج على الأقل مراجع واحد
   - المالك (SAMER-RCC) يوافق على PR الكبيرة

---

## 🚨 نصائح مهمة

### ✅ افعل:
- اكتب رسائل commit واضحة
- اختبر الكود قبل الـ Push
- اتبع معايير الكود
- وثق التغييرات بشكل جيد
- اطلب مساعدة عند الحاجة

### ❌ لا تفعل:
- لا تدمج مع main مباشرة
- لا تغيّر الكود بدون اختبارات
- لا تضف dependencies عشوائية
- لا تتجاهل comments المراجعين
- لا تترك commits بدون وصف

---

## 📞 الحصول على المساعدة

### أسئلة عامة
- 💬 [GitHub Discussions](https://github.com/SAMER-RCC/vortexia-revit-poc-2027/discussions)

### تقارير الأخطاء
- 🐛 [GitHub Issues](https://github.com/SAMER-RCC/vortexia-revit-poc-2027/issues)

### التواصل المباشر
- 📧 sameralfarwi@gmail.com

---

## 🎓 موارد إضافية

- [GitHub Guides](https://guides.github.com)
- [Conventional Commits](https://www.conventionalcommits.org)
- [.NET Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/naming-conventions)
- [Revit API Guide](https://www.autodesk.com/developer/revit)

---

## 🙏 شكر لمساهمتك!

نقدر جهودك وإضافتك للمشروع! 🌟

**آخر تحديث:** 2026-09-11
