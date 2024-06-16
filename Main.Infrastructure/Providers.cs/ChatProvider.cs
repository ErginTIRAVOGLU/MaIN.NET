using MaIN.Infrastructure.Models;
using MaIN.Infrastructure.Providers.cs.Abstract;
using MongoDB.Driver;

namespace MaIN.Infrastructure.Providers.cs;

public class ChatProvider(IMongoDatabase database, string collectionName) : IChatProvider
{
    private readonly IMongoCollection<ChatDocument> _chats = database.GetCollection<ChatDocument>(collectionName);

    public async Task<IEnumerable<ChatDocument>> GetAllChats()
    {
        return await _chats.Find(chat => true).ToListAsync();
    }

    public async Task<ChatDocument> GetChatById(string id)
    {
        return await _chats.Find<ChatDocument>(chat => chat.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddChat(ChatDocument chat)
    {
        await _chats.InsertOneAsync(chat);
    }

    public async Task UpdateChat(string id, ChatDocument chat)
    {
        await _chats.ReplaceOneAsync(x => x.Id == id, chat);
    }

    public async Task DeleteChat(string id)
    {
        await _chats.DeleteOneAsync(x => x.Id == id);
    }
}