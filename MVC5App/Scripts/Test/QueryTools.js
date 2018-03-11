document.write("pizza");
querySelect();
function querySelect() {

    document.write('pizza3');
    var knex = require('../../.bin/knex');
    document.write('pizza43');
    knex.select().from('Movies').timeout(1000);
    document.write(knex.toString());
    document.write(knex.select().from('Movies').timeout(1000).toString());
    document.write('pizza2');
}