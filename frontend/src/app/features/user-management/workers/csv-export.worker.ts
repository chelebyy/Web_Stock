/// <reference lib="webworker" />

interface User {
  id?: number;
  adi: string;
  soyadi: string;
  sicil: string;
  role?: string; // rol adı opsiyonel
}

function convertToCSV(users: User[]): string {
  if (!users || users.length === 0) {
    return '';
  }

  const header = ['ID', 'Adı', 'Soyadı', 'Sicil', 'Rol'];
  const rows = users.map(user => 
    [
      user.id ?? '',
      user.adi,
      user.soyadi,
      user.sicil,
      user.role ?? 'N/A'
    ].join(',')
  );

  return [header.join(','), ...rows].join('\r\n');
}


addEventListener('message', ({ data }: { data: User[] }) => {
  if (data && data.length > 0) {
    const csvData = convertToCSV(data);
    postMessage(csvData);
  } else {
    postMessage('');
  }
}); 