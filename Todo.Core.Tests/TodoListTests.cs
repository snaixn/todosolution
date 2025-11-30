using Xunit;
using Todo.Core;
using System.Linq;
namespace Todo.Core.Tests
{
    public class TodoListTests
    {
        [Fact]
        public void Add_IncreasesCount()
        {
            var list = new TodoList();
            list.Add(" task ");
            Assert.Equal(1, list.Count);
            Assert.Equal("task", list.Items.First().Title);
        }
        [Fact]
        public void Remove_ById_Works()
        {
            var list = new TodoList();
            var item = list.Add("a");
            Assert.True(list.Remove(item.Id));
            Assert.Equal(0, list.Count);
        }
        [Fact]
        public void Find_ReturnsMatches()
        {
            var list = new TodoList();
            list.Add("Buy milk");
            list.Add("Read book");
            var found = list.Find("buy").ToList();
            Assert.Single(found);
            Assert.Equal("Buy milk", found[0].Title);
        }
        [Fact]
        public void SaveAndLoad_PreservesAllData()
        {
            var list = new TodoList();
            var item1 = list.Add("Task 1");
            item1.MarkDone();
            var item2 = list.Add("Task 2");

            var tempFile = Path.GetTempFileName();

            try
            {
                // Сохраняем
                list.Save(tempFile);

                // Загружаем
                var loaded = TodoList.Load(tempFile);

                // Проверяем
                Assert.Equal(2, loaded.Count);
                var items = loaded.Items.ToList();
                Assert.Equal("Task 1", items[0].Title);
                Assert.True(items[0].IsDone);
                Assert.Equal(item1.Id, items[0].Id); // Id сохранён!
                Assert.Equal("Task 2", items[1].Title);
                Assert.False(items[1].IsDone);
                Assert.Equal(item2.Id, items[1].Id);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void Load_NonExistentFile_Throws()
        {
            Assert.Throws<FileNotFoundException>(() => TodoList.Load("no-such-file.json"));
        }
    }
}
