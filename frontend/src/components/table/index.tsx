import type { TableProps } from '../../interfaces/table/tableProps'

function Table({ data }: TableProps) {

    const columnHeaders = Object.keys(data[0] || {})

    return (
        <table className="min-w-full rounded-lg p-7 shadow-[0_4px_4px_rgba(0,0,0,0.25)] text-sm">
            <thead>
                <tr className="text-left">
                    {columnHeaders.map((header) => (
                        <th key={header} className="px-4 py-2">{header}</th>
                    ))}
                </tr>
            </thead>
            <tbody>

                {data.map((row, index) => {
                    return (
                        <tr key={index} className="border-t">
                            {columnHeaders.map((header) => {
                                const element = (row as Record<string, string>)[header]

                                return (
                                    <td key={header} className={`px-4 py-2`}>
                                        {element}
                                    </td>
                                )
                            })}
                        </tr>
                    )
                })}
            </tbody>
        </table>
    )
}
export default Table