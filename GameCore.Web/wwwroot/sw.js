// GameCore Service Worker for PWA functionality
const CACHE_NAME = 'gamecore-v1.0.0';
const urlsToCache = [
  '/',
  '/css/site.css',
  '/js/app.js',
  '/lib/bootstrap/dist/css/bootstrap.min.css',
  '/lib/jquery/dist/jquery.min.js',
  '/images/logo.svg',
  '/images/icons/icon-192x192.png',
  '/images/icons/icon-512x512.png',
  '/manifest.json'
];

// Install event - cache resources
self.addEventListener('install', event => {
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then(cache => {
        console.log('Opened cache');
        return cache.addAll(urlsToCache);
      })
  );
});

// Fetch event - serve from cache when offline
self.addEventListener('fetch', event => {
  event.respondWith(
    caches.match(event.request)
      .then(response => {
        // Return cached version or fetch from network
        if (response) {
          return response;
        }
        return fetch(event.request);
      }
    )
  );
});

// Activate event - cleanup old caches
self.addEventListener('activate', event => {
  event.waitUntil(
    caches.keys().then(cacheNames => {
      return Promise.all(
        cacheNames.map(cacheName => {
          if (cacheName !== CACHE_NAME) {
            console.log('Deleting old cache:', cacheName);
            return caches.delete(cacheName);
          }
        })
      );
    })
  );
});

// Background sync for offline actions
self.addEventListener('sync', event => {
  if (event.tag === 'background-sync') {
    event.waitUntil(doBackgroundSync());
  }
});

// Push notifications
self.addEventListener('push', event => {
  const options = {
    body: event.data.text(),
    icon: '/images/icons/icon-192x192.png',
    badge: '/images/icons/badge-72x72.png',
    vibrate: [100, 50, 100],
    data: {
      dateOfArrival: Date.now(),
      primaryKey: 1
    },
    actions: [
      {
        action: 'explore',
        title: '查看詳情',
        icon: '/images/icons/checkmark.png'
      },
      {
        action: 'close',
        title: '關閉',
        icon: '/images/icons/xmark.png'
      }
    ]
  };

  event.waitUntil(
    self.registration.showNotification('GameCore', options)
  );
});

// Notification click handler
self.addEventListener('notificationclick', event => {
  event.notification.close();

  if (event.action === 'explore') {
    event.waitUntil(
      clients.openWindow('/notifications')
    );
  } else if (event.action === 'close') {
    // Just close the notification
  } else {
    // Default action - open app
    event.waitUntil(
      clients.openWindow('/')
    );
  }
});

// Background sync function
async function doBackgroundSync() {
  try {
    // Sync offline pet interactions
    const petInteractions = await getStoredPetInteractions();
    for (const interaction of petInteractions) {
      await syncPetInteraction(interaction);
    }
    
    // Sync offline forum posts
    const forumPosts = await getStoredForumPosts();
    for (const post of forumPosts) {
      await syncForumPost(post);
    }
    
    console.log('Background sync completed');
  } catch (error) {
    console.error('Background sync failed:', error);
  }
}

// Helper functions for offline data management
async function getStoredPetInteractions() {
  // Get stored interactions from IndexedDB
  return [];
}

async function syncPetInteraction(interaction) {
  // Sync pet interaction with server
  return fetch('/api/pet/sync-interaction', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(interaction)
  });
}

async function getStoredForumPosts() {
  // Get stored posts from IndexedDB
  return [];
}

async function syncForumPost(post) {
  // Sync forum post with server
  return fetch('/api/forum/sync-post', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(post)
  });
} 